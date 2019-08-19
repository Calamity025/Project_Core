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
                var user = await _db.UserInfos.GetAsync(history.UserId);
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
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (user == null || slot == null)
            {
                throw new NotFoundException();
            }
            if (user.FollowingSlots.Contains(slot))
            {
                throw new DatabaseException("This user already follows this slot");
            }
            user.FollowingSlots.Add(slot);
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
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (user == null || slot == null)
            {
                throw new NotFoundException();
            }
            user.FollowingSlots.Remove(slot);
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
                .Include(x => x.BetSlots)
                .Include(x => x.PlacedSlots)
                .Include(x => x.WonSlots)
                .FirstOrDefaultAsync();
            if (profile == null)
            {
                throw new NotFoundException();
            }
            ProfileDTO profileDto = _mapper.Map<ProfileDTO>(profile);
            profileDto.FollowingSlots = Map(profile.FollowingSlots);
            profileDto.BetSlots = Map(profile.BetSlots);
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
