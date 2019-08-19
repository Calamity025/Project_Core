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
            newSlot.EndTime = DateTime.Now;
            newSlot.EndTime = newSlot.EndTime.AddDays(30);
            newSlot.UserId = seller.Id;
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
            Slot slot = await _db.Slots.GetAll().Where(x => x.Id == slotId)
                .Include(x => x.SlotTags).FirstOrDefaultAsync();
            var user = await _db.UserInfos.GetAsync(userId);
            List<BetHistory> histories = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == slotId).ToListAsync();
            if (slot == null || user == null)
            {
                throw new NotFoundException();
            }
            slot.SlotTags = null;
            await _db.SaveChangesAsync();
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

        public async Task UpdateGeneralInfo(int slotId, SlotUpdateDTO slotInfo)
        {
            Slot slot = await _db.Slots.GetAll().Where(x => x.Id == slotId)
                .Include(x => x.SlotTags).FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new NotFoundException();
            }
            slot.Name = slotInfo.Name ?? slot.Name;
            slot.Description = slot.Description ?? slot.Description;
            if (slotInfo.SlotTagIds != null)
            {
                var tags = await _db.Tags.GetAll()
                    .Where(x => slotInfo.SlotTagIds.Contains(x.Id)).ToListAsync();
                slot.SlotTags = tags;
            }
            if (slotInfo.CategoryId != -1)
            {
                var category = await _db.Categories.GetAsync(slotInfo.CategoryId);
                slot.Category = category;
                slot.CategoryId = category.Id;
            }
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

        public async Task UpdateStatus(int slotId, Status.SlotStatus status)
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

        public async Task MakeBet(int slotId, int userId, decimal bet)
        {
            var slot = await _db.Slots.GetAsync(slotId);
            var user = await _db.UserInfos.GetAsync(userId);
            if (slot == null || user == null)
            {
                throw new NotFoundException();
            }
            var maxSlotBet = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == slotId)
                .OrderByDescending(x => x.Price)
                .FirstOrDefaultAsync();
            if (maxSlotBet == null && (bet < slot.StarterPrice && bet - slot.MinBet < slot.StarterPrice) ||
                maxSlotBet != null && (bet < maxSlotBet.Price || bet - slot.MinBet < maxSlotBet.Price))
            {
                throw new ArgumentException("Bet cannot be lower than actual price");
            }
            if (bet > user.Balance)
            {
                throw new ArgumentException("Bet cannot be greater than user balance");
            }
            _db.BetHistories.Create(new BetHistory() { Price = bet, Slot = slot, UserId = userId });
            user.BetSlots.Add(slot);
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
            var bets = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == slotId && x.UserId == userId).ToListAsync();
            var user = await _db.UserInfos.GetAsync(userId);
            if (bets == null || user == null)
            {
                throw new NotFoundException();
            }
            foreach (var betHistory in bets)
            {
                user.PlacedSlots.Remove(betHistory.Slot);
                _db.Update(user);
                _db.BetHistories.Delete(betHistory);
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when deleting bet", e);
            }
        }

        public async Task CloseSlots(IEnumerable<Slot> slots)
        {
            foreach (var slot in slots)
            {
                slot.Status = Status.SlotStatus.Finished.ToString();
                _db.Update(slot);
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("", e);
            }
        }
    }
}
