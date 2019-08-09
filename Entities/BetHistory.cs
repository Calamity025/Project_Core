using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class BetHistory
    {
        public int Id { get; set; }
        public Slot Slot { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
    }
}
