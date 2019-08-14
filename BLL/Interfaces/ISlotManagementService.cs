using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface ISlotManagementService : IDisposable
    {
        Task<int> CreateSlot(int userId, SlotCreationDTO slotDTO);
        Task<string> DeleteSlot(int slotId, int userId);
        Task AddImageLink(int id, string link);
        Task AddTags(int slotId, IEnumerable<int> tagIds);
        Task RemoveTags(int slotId, IEnumerable<int> tagIds);
        Task UpdateGeneralInfo(int slotId, SlotGeneralInfoDTO slotInfo);
        Task UpdateStatus(int slotId, Status status);
        Task AddToUserFollowingList(int userId, int slotId);
        Task RemoveFromUserFollowingList(int userId, int slotId);
        Task MakeBet(int slotId, int userId, decimal bet);
        Task UndoBet(int slotId, int userId);
    }
}
