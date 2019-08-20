using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IMapper _mapper;

        public ProfileManagementService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task CreateProfile(int userId, ProfileCreationDTO profile)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            _db.Update(user);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating profile", e);
            }
        }

        public async Task AddAvatarLink(int userId, string link)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            user.ImageLink = link;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when adding avatar link", e);
            }
        }

        public async Task AddToWonSlotsList(IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                var history = await _db.BetHistories.GetAll()
                    .Where(x => x.Slot.Id == slot.Id)
                    .OrderByDescending(x => x.Price)
                    .FirstOrDefaultAsync();
                if (history == null)
                {
                    throw new NotFoundException();
                }
                var user = await _db.UserInfos.GetAsync(history.BetUserInfoId);
                if (user == null)
                {
                    throw new NotFoundException();
                }
                user.Balance -= history.Price;
                user.WonSlots.Add(history.Slot);
                _db.Update(user);
            }
            await _db.SaveChangesAsync();
        }

        public async Task AddToUserFollowingList(int userId, int slotId)
        {
            var userTask = _db.UserInfos.GetAsync(userId);
            var slotTask = _db.Slots.GetAsync(slotId);
            var user = await userTask;
            var slot = await slotTask;
            if (user == null || slot == null)
            {
                throw new NotFoundException();
            }
            if (user.FollowingSlots.Any(x => x.Slot == slot))
            {
                throw new DatabaseException("This user already follows this slot");
            }
            _db.UserInfos.Follow(new FollowingSlots(){Slot = slot, FollowingUserInfo = user});
            _db.Update(user);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when adding to following list", e);
            }
        }

        public async Task RemoveFromUserFollowingList(int userId, int slotId)
        {
            var userTask = _db.UserInfos.GetAsync(userId);
            var slotTask = _db.Slots.GetAsync(slotId);
            var user = await userTask;
            var slot = await slotTask;
            if (user == null || slot == null)
            {
                throw new NotFoundException();
            }
            _db.UserInfos
                .Unfollow(user.FollowingSlots.FirstOrDefault(x => x.Slot == slot));
            _db.Update(user);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when removing from following list", e);
            }
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetFollowingSlots(int userId)
        {
            var profile = await _db.UserInfos.GetAsync(userId);
            if (profile == null)
            {
                throw new NotFoundException();
            }
            List<SlotMinimumDTO> res = new List<SlotMinimumDTO>();
            foreach (var slot in profile.FollowingSlots)
            {
                res.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            return res;
        }

        public async Task<ProfileDTO> GetProfile(int id)
        {
            var profile = await _db.UserInfos.GetAll().Where(x => x.Id == id)
                .Include(x => x.FollowingSlots)
                .ThenInclude(x => x.Slot)
                .Include(x => x.BetSlots)
                .ThenInclude(x => x.Slot)
                .Include(x => x.PlacedSlots)
                .Include(x => x.WonSlots)
                .FirstOrDefaultAsync();
            if (profile == null)
            {
                throw new NotFoundException();
            }
            ProfileDTO profileDto = _mapper.Map<ProfileDTO>(profile);
            profileDto.FollowingSlots = Map(profile.FollowingSlots.Select(x => x.Slot));
            profileDto.BetSlots = Map(profile.BetSlots.Select(x => x.Slot));
            profileDto.WonSlots = Map(profile.WonSlots);
            profileDto.PlacedSlots = Map(profile.PlacedSlots);
            return profileDto;
        }

        private ICollection<SlotMinimumDTO> Map(IEnumerable<Slot> list)
        {
            List<SlotMinimumDTO> slots = new List<SlotMinimumDTO>();
            foreach (var slot in list)
            {
                slots.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            return slots;
        }
    }
}
