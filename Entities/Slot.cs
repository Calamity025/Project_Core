using System;
using System.Collections.Generic;

namespace Entities
{
    public class Slot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserInfoId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public decimal StarterPrice { get; set; }
        public decimal MinBet { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<SlotTag> SlotTags { get; } = new List<SlotTag>();
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public ICollection<FollowingSlots> Following { get; } = 
            new List<FollowingSlots>();
        public ICollection<BetHistory> Betters { get; } = 
            new List<BetHistory>();
    }
}
