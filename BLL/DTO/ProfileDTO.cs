using System.Collections.Generic;

namespace BLL.DTO
{
    public class ProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageLink { get; set; }
        public ICollection<SlotMinimumDTO> FollowingSlots { get; set; }
        public ICollection<SlotMinimumDTO> BetSlots { get; set; }
        public ICollection<SlotMinimumDTO> PlacedSlots { get; set; }
        public ICollection<SlotMinimumDTO> WonSlots { get; set; }
    }
}
