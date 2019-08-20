using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;
        private readonly ISlotManagementService _slotManagementService;

        public IdentityService(IDataUnitOfWork db, IIdentityUnitOfWork identity, IMapper mapper, ISlotManagementService slotManagementService)
        {
            _db = db;
            _identity = identity;
            _mapper = mapper;
            _slotManagementService = slotManagementService;
        }

        public async Task Register(IdentityCreationDTO identityCreation)
        {
            if (await _identity.UserManager.FindByNameAsync(identityCreation.UserName) != null)
            {
                throw new DatabaseException();
            }
            User user = _mapper.Map<User>(identityCreation);
            UserInfo info = new UserInfo(){User = user, Id = user.Id};
            _db.UserInfos.Create(info);
            user.UserInfo = info;
            try
            {
                await _identity.UserManager.CreateAsync(user, identityCreation.Password);
                await _identity.UserManager
                    .AddClaimAsync(user, new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
                await _identity.UserManager
                    .AddClaimAsync(user, new Claim("Id", user.Id.ToString()));
                await _identity.UserManager.AddToRoleAsync(user, "user");
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating identity", e);
            }
        }

        public async Task<ClaimsIdentity> Login(LoginInfoDTO loginInfoDto, string authType)
        {
            User user = await _identity.UserManager.FindByNameAsync(loginInfoDto.Login);
            if (user != null && await _identity.UserManager.CheckPasswordAsync(user, loginInfoDto.Password))
            {
                var claimsTask = _identity.UserManager.GetClaimsAsync(user);
                var rolesTask = _identity.UserManager.GetRolesAsync(user);
                var claims = await claimsTask;
                foreach (var role in await rolesTask)
                {
                    var roleClaim = new Claim(ClaimTypes.Role, role);
                    claims.Add(roleClaim);
                }
                return new ClaimsIdentity(claims, authType);
            }
            return null;
        }

        public async Task<UserDTO> GetCurrentUser(string name)
        {
            User user = await _identity.UserManager.FindByNameAsync(name);
            UserDTO res = _mapper.Map<UserDTO>(user);
            UserInfo info = await _db.UserInfos.GetAll()
                .Where(x => x.Id == user.Id)
                .Include(x => x.FollowingSlots)
                .FirstOrDefaultAsync();
            res.AvatarLink = info.ImageLink;
            res.FollowingSlots = info.FollowingSlots.Select(x => x.SlotId);
            res.Roles = await _identity.UserManager.GetRolesAsync(user);
            res.Balance = info.Balance;
            return res;
        }

        public async Task DeleteIdentity(string name)
        {
            var user = await _identity.UserManager.FindByNameAsync(name);
            if (user == null)
            {
                throw new NotFoundException();
            }
            var slots = await _db.Slots.GetAll().Where(x => x.UserInfoId == user.Id).ToListAsync();
            foreach (var slot in slots)
            {
                await _slotManagementService.DeleteSlot(slot.Id, user.Id);
            }
            var info = await _db.UserInfos.GetAll().Where(x => x.Id == user.Id)
                .Include(x => x.FollowingSlots)
                .Include(x => x.BetSlots)
                .Include(x => x.PlacedSlots)
                .Include(x => x.WonSlots)
                .FirstOrDefaultAsync();
            _db.UserInfos.UnfollowRange(info.FollowingSlots);
            _db.BetHistories.DeleteRange(info.BetSlots);
            info.PlacedSlots = null;
            info.WonSlots = null;
            _db.UserInfos.Delete(info);
            await _db.SaveChangesAsync();
            await _identity.UserManager.DeleteAsync(user);
        }

        public async Task AddToRole(string userName, string role)
        {
            var user = await _identity.UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _identity.UserManager.AddToRoleAsync(user, role);
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _identity.UserManager.Users.ToListAsync();
            List<UserDTO> userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                var userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Roles = await _identity.UserManager.GetRolesAsync(user);
                userDTOs.Add(userDTO);
            }
            return userDTOs;
        }

        public async Task<decimal> AddMoney(int userId, decimal value)
        {
            var user = await _db.UserInfos.GetAsync(userId);
            user.Balance += value;
            _db.Update(user);
            try
            {
                await _db.SaveChangesAsync();
                return user.Balance;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when adding money to balance", e);
            }
        }
    }
}
