using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class SlotTag
    {
        public int SlotId { get; set; }
        public Slot Slot { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
