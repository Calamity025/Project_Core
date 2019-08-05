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
        Task DeleteSlot(int id);
        Task AddImageLink(int id, string link);
        Task AddTags(int slotId, IEnumerable<int> tagIds);
        Task RemoveTags(int slotId, IEnumerable<int> tagIds);
        Task UpdateGeneralInfo(int slotId, SlotGeneralInfoDTO slotInfo);
        Task UpdateStatus(int slotId, Status status);

    }
}
