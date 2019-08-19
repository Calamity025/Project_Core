using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class SlotUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ICollection<int> SlotTagIds { get; set; }
    }
}
