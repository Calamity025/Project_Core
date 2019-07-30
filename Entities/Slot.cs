using System;
using System.Collections.Generic;

namespace Entities
{
    public class Slot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public decimal Price { get; set; }
        public decimal MinBet { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> SlotTags { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }


        public Slot()
        {
            SlotTags = new List<Tag>();
        }
    }
}
