using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;

        public ProfileManagementService(IDataUnitOfWork db, IMapper mapper, IIdentityUnitOfWork identity)
        {
            _db = db;
            _mapper = mapper;
            _identity = identity;
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
            user.DeliveryAddress = profile.DeliveryAddress;
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
                    .Include(x => x.Slot)
                    .Where(x => x.Slot.Id == slot.Id)
                    .OrderByDescending(x => x.Price)
                    .Take(1)
                    .FirstAsync();
                var user = await _db.UserInfos.GetAsync(history.UserId);
                if (user == null)
                {
                    throw new NotFoundException();
                }

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
    }
}
