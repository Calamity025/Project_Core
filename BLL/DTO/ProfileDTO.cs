using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace BLL.DTO
{
    public class ProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeliveryAddress { get; set; }
        public string ImageLink { get; set; }
        public virtual ICollection<SlotMinimumDTO> FollowingSlots { get; set; }
        public virtual ICollection<SlotMinimumDTO> BetSlots { get; set; }
        public virtual ICollection<SlotMinimumDTO> PlacedSlots { get; set; }
        public virtual ICollection<SlotMinimumDTO> WonSlots { get; set; }
    }
}
