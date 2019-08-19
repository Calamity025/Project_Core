using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;
using Entities;

namespace BLL.Interfaces
{
    public interface ISlotManagementService
    {
        Task<int> CreateSlot(int userId, SlotCreationDTO slotDTO);
        Task<string> DeleteSlot(int slotId, int userId);
        Task AddImageLink(int id, string link);
        Task UpdateGeneralInfo(int slotId, SlotUpdateDTO slotInfo);
        Task UpdateStatus(int slotId, Status.SlotStatus status);
        Task MakeBet(int slotId, int userId, decimal bet);
        Task UndoBet(int slotId, int userId);
        Task CloseSlots(IEnumerable<Slot> slots);
    }
}
