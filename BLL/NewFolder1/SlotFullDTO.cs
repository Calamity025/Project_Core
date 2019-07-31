using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class SlotFullDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal MinBet { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<int> SlotTagsId { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
    }
}
