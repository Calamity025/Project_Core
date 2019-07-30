using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;

namespace BLL
{
    public class SlotManagementService : ISlotManagementService
    {
        private readonly IUnit _db;
        public SlotManagementService(IUnit unit)
        {
            _db = unit;
        }

        public async Task<bool> AddSlot(Slot newSlot)
        {
            Task<Category> newSlotCategory = _db.Categories.GetAsync(newSlot.CategoryId);
            Task<UserInfo> newSlotSeller = _db.UserInfos.GetAsync(newSlot.UserId);
            Task<List<Tag>> tagList = _db.Tags.GetAll().Where(x => newSlot.SlotTags.Contains(x)).ToListAsync();
            await Task.WhenAll(newSlotCategory, newSlotSeller, tagList);
            Slot slot = new Slot()
            {
                Name = newSlot.Name,
                UserId = newSlot.UserId,
                UserInfo = newSlotSeller.Result,
                Price = newSlot.Price,
                MinBet = newSlot.MinBet,
                EndTime = newSlot.EndTime,
                Status = Status.SlotStatus.Started.ToString(),
                Category = newSlotCategory.Result,
                SlotTags = tagList.Result,
                Description = newSlot.Description,
                ImageLink = newSlot.ImageLink
            };
            try
            {
                _db.Slots.Create(slot);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSlot(int id)
        {
            try
            {
                await _db.Slots.DeleteAsync(id);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeSlotInfo(int id, string newName = null, int newCategoryId = 0, string newDescription = null, string newImageLink = null)
        {
            var slot = await _db.Slots.GetAsync(id);
            slot.Name = newName ?? slot.Name;
            slot.Category = await _db.Categories.GetAsync(newCategoryId) ?? slot.Category;
            slot.Description = newDescription ?? slot.Description;
            slot.ImageLink = newImageLink ?? slot.ImageLink;
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddSlotTags(int id, ICollection<int> tagsToAdd)
        {
            if (tagsToAdd != null)
            {
                var slot = _db.Slots.GetAsync(id);
                var tags = _db.Tags.GetAll().Where(x => tagsToAdd.Contains(x.Id)).ToListAsync();
                await Task.WhenAll(slot, tags);
                if (slot.Result.SlotTags.Concat(tags.Result) is ICollection<Tag> newTags)
                {
                    slot.Result.SlotTags = newTags;
                }

                try
                {
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> RemoveSlotTags(int id, ICollection<int> tagsToRemove)
        {
            if (tagsToRemove != null)
            {
                var slot = await _db.Slots.GetAsync(id);
                var tags = slot.SlotTags.Where(x => !tagsToRemove.Contains(x.Id));
                if (tags is ICollection<Tag> newTags)
                {
                    slot.SlotTags = newTags;
                }
                try
                {
                    _db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> ChangeSlotCategory(int id, int categoryId)
        {
            var slot = _db.Slots.GetAsync(id);
            var category = _db.Categories.GetAsync(categoryId);
            await Task.WhenAll(slot, category);
            slot.Result.Category = category.Result;

            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeStatus(int id, Status.SlotStatus status)
        {
            var slot = await _db.Slots.GetAsync(id);
            slot.Status = status.ToString();
            if (status == Status.SlotStatus.Closed)
            {
                slot.EndTime = DateTime.UtcNow;
            }
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProlongEndTime(int id, TimeSpan timeToAdd)
        {
            var slot = await _db.Slots.GetAsync(id);
            slot.EndTime += timeToAdd;
            try
            {
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Flag: Has Dispose already been called?
        bool _disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _db.Dispose();
            }

            // Free any unmanaged objects here.
            //
            _disposed = true;
        }

        ~SlotManagementService()
        {
            Dispose(false);
        }
    }
}
