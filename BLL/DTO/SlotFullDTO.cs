using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace BLL.DTO
{
    public class SlotFullDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Step { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public Category Category { get; set; }
        public ICollection<Tag> SlotTags { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public UserDTO User { get; set; }
    }
}
