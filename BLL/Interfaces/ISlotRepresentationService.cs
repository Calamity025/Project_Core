using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using Entities;

namespace BLL.Interfaces
{
    public interface ISlotRepresentationService : IDisposable
    {
        Task<Page> GetPage(int pageNumber, int slotsOnPage);
        Task<SlotFullDTO> GetSlot(int id);
        Task<decimal> GetSlotPrice(int id);
        Task<decimal> GetUserBet(int id, int userId);
        Task<Page> GetByCategory(int categoryId, 
            int pageNumber, int slotsOnPage);
        Task<Page> GetByTags(IEnumerable<int> tagIds, 
            int pageNumber, int slotsOnPage);
        Task<Page> GetByName(string query, int pageNumber, 
            int slotsOnPage);
        Task<IEnumerable<SlotMinimumDTO>> GetUserFollowingSlots(int userId);
        Task<IEnumerable<SlotMinimumDTO>> GetUserSlots(int userId);
        Task<string> GetSlotImage(int slotId);
        Task<IEnumerable<Slot>> GetExpiredSlots();

    }
}
