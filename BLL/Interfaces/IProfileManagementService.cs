using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using Entities;

namespace BLL.Interfaces
{
    public interface IProfileManagementService
    {
        Task CreateProfile(int userId, ProfileCreationDTO profile);
        Task AddAvatarLink(int userId, string link);
        Task AddToWonSlotsList(IEnumerable<Slot> slots);
        Task AddToUserFollowingList(int userId, int slotId);
        Task RemoveFromUserFollowingList(int userId, int slotId);
        Task<IEnumerable<SlotMinimumDTO>> GetFollowingSlots(int userId);
    }
}
