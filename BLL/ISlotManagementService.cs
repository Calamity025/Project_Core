using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace BLL
{
    public interface ISlotManagementService : IDisposable
    {
        Task<bool> AddSlot(Slot newSlot);
        Task<bool> DeleteSlot(int id);
        Task<bool> ChangeSlotInfo(int id, string newName = null, int newCategoryId = 0, string newDescription = null,
            string newImageLink = null);
        Task<bool> AddSlotTags(int id, ICollection<int> tagsToAdd);
        Task<bool> RemoveSlotTags(int id, ICollection<int> tagsToRemove);
        Task<bool> ChangeSlotCategory(int id, int categoryId);
        Task<bool> ChangeStatus(int id, Status.SlotStatus status);
        Task<bool> ProlongEndTime(int id, TimeSpan timeToAdd);
    }
}
