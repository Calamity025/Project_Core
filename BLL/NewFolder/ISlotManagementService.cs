using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ISlotManagementService : IDisposable
    {
        Task CreateSlot(SlotCreationDTO slotDTO);
        Task DeleteSlot(int id);
        Task AddTags(int slotId, IEnumerable<int> tagIds);
        Task RemoveTags(int slotId, IEnumerable<int> tagIds);
        Task UpdateGeneralInfo(int slotId, SlotGeneralInfoDTO slotInfo);
        Task UpdateStatus(int slotId, Status status);
    }
}
