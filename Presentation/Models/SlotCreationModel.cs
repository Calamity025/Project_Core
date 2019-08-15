﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class SlotCreationModel
    {
        public int UserId { get; set; }
        public string  Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public decimal Step { get; set; }
        public DateTime EndTime { get; set; }
        public int[] SlotTagIds { get; set; }
        public string Description { get; set; }
    }
}
