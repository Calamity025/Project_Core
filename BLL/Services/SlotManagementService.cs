using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class SlotManagementService : ISlotManagementService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IMapper _mapper;

        public SlotManagementService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> CreateSlot(int userId, SlotCreationDTO slotDTO)
        {
            UserInfo seller = await _db.UserInfos.GetAsync(userId);
            Category category = await _db.Categories.GetAsync(slotDTO.CategoryId);
            if (seller == null || category == null)
            {
                throw new NotFoundException();
            }

            List<Tag> tags = await _db.Tags.GetAll().Where(x => slotDTO.SlotTagIds.Contains(x.Id)).ToListAsync();
            var newSlot = _mapper.Map<Slot>(slotDTO);
            newSlot.Category = category;
            newSlot.SlotTags = tags;
            newSlot.Status = Status.SlotStatus.Started.ToString();
            _db.BetHistories.Create(new BetHistory() { Slot = newSlot, UserId = seller.Id, Price = slotDTO.Price });
            _db.Slots.Create(newSlot);
            seller.PlacedSlots.Add(newSlot);

            try
            {
                await _db.SaveChangesAsync();
                return newSlot.Id;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating slot", e);
            }
        }

        public async Task<string> DeleteSlot(int slotId, int userId)
        {
            Slot slot = await _db.Slots.GetAsync(slotId);
            var user = await _db.UserInfos.GetAsync(userId);
            List<BetHistory> histories = await _db.BetHistories.GetAll().Where(x => x.Slot.Id == slotId).ToListAsync();
            if (slot == null || user == null)
            {
                throw new NotFoundException();
            }

            _db.Slots.Delete(slot);
            user.PlacedSlots.Remove(slot);
            foreach (var betHistory in histories)
            {
                _db.BetHistories.Delete(betHistory);
            }

            try
            {
                await _db.SaveChangesAsync();
                return slot.ImageLink;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when deleting slot", e);
            }
        }

        public async Task AddImageLink(int id, string link)
        {
            Slot slot = await _db.Slots.GetAsync(id);
            if (slot == null)
            {
                throw new NotFoundException();
            }
            slot.ImageLink = link;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when adding image link", e);
            }
        }

        public async Task AddTags(int slotId, IEnumerable<int> tagIds)
        {
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (slot == null)
            {
                throw new NotFoundException();
            }

            ICollection<Tag> tags = await _db.Tags.GetAll().Where(x => tagIds.Contains(x.Id)).ToListAsync();
            slot.SlotTags = slot.SlotTags.Concat(tags) as ICollection<Tag>;
            _db.Update(slot);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating slot", e);
            }
        }

        public async Task RemoveTags(int slotId, IEnumerable<int> tagIds)
        {
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (slot == null)
            {
                throw new NotFoundException();
            }

            IEnumerable<Tag> tags = slot.SlotTags.Where(x => !tagIds.Contains(x.Id));

            slot.SlotTags = tags as ICollection<Tag>;
            _db.Update(slot);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating slot", e);
            }
        }

        public async Task UpdateGeneralInfo(int slotId, SlotGeneralInfoDTO slotInfo)
        {
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (slot == null)
            {
                throw new NotFoundException();
            }

            slot.Name = slotInfo.Name;
            slot.Description = slot.Description;

            _db.Update(slot);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating slot", e);
            }
        }

        public async Task UpdateStatus(int slotId, Status status)
        {
            Slot slot = await _db.Slots.GetAsync(slotId);
            if (slot == null)
            {
                throw new NotFoundException();
            }

            slot.Status = status.ToString();
            _db.Update(slot);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating slot", e);
            }
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

        public async Task MakeBet(int slotId, int userId, decimal bet)
        {
            var slot = await _db.Slots.GetAsync(slotId);
            var user = await _db.UserInfos.GetAsync(userId);
            if (slot == null || user == null)
            {
                throw new NotFoundException();
            }

            var maxSlotBet = await _db.BetHistories.GetAll().Where(x => x.Slot.Id == slotId).MaxAsync(x => x.Price);
            if (bet < maxSlotBet || bet - slot.MinBet < maxSlotBet)
            {
                throw new ArgumentException("Bet cannot be lower than actual price");
            }

            user.BetSlots.Add(slot);
            _db.BetHistories.Create(new BetHistory(){Price = bet, Slot = slot, UserId = userId});
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when making a bet", e);
            }
        }

        public async Task UndoBet(int slotId, int userId)
        {
            var bet = await _db.BetHistories.GetAll().FirstOrDefaultAsync(x => x.Slot.Id == slotId && x.UserId == userId);
            var user = await _db.UserInfos.GetAsync(userId);
            if (bet == null || user == null)
            {
                throw new NotFoundException();
            }

            user.BetSlots.Remove(bet.Slot);
            _db.BetHistories.Delete(bet);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when deleting bet");
            }
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
                _db.Dispose();
            }

            disposed = true;
        }

        ~SlotManagementService()
        {
            Dispose(false);
        }
    }
}
