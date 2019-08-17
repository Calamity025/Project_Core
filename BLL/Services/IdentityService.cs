using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;

        public IdentityService(IDataUnitOfWork db, IIdentityUnitOfWork identity, IMapper mapper)
        {
            _db = db;
            _identity = identity;
            _mapper = mapper;
        }

        public async Task<UserDTO> Register(IdentityCreationDTO identityCreation)
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
                await _identity.UserManager.AddClaimAsync(user, new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
                await _identity.UserManager.AddClaimAsync(user, new Claim("Id", user.Id.ToString()));
                await _identity.UserManager.AddToRoleAsync(user, "user");
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception e)
            {
                _db.UserInfos.Delete(info);
                await _db.SaveChangesAsync();
                throw new DatabaseException("Error when creating identity", e);
            }
        }

        public async Task<ClaimsIdentity> Login(LoginInfo loginInfo, string authType)
        {
            User user = await _identity.UserManager.FindByNameAsync(loginInfo.Login);
            if (user != null && await _identity.UserManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                var claims = await _identity.UserManager.GetClaimsAsync(user);
                foreach (var role in await _identity.UserManager.GetRolesAsync(user))
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
                .Where(x => x.Id == user.Id).Include(x => x.FollowingSlots).FirstAsync();
            res.AvatarLink = info.ImageLink;
            res.FollowingSlots = info.FollowingSlots.Select(x => x.Id);
            res.Roles = await _identity.UserManager.GetRolesAsync(user);
            return res;
        }

        public async Task AddToRole(int id, string roleName)
        {

        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _identity.Dispose();
            }

            disposed = true;
        }

        ~IdentityService()
        {
            Dispose(false);
        }
    }
}
