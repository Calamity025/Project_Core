using System;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class SlotCreationDTO
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public decimal Step { get; set; }
        public DateTime EndTime { get; set; }
        public int CategoryId { get; set; }
        public ICollection<int> SlotTagIds { get; set; }
        public string Description { get; set; }
    }
}
