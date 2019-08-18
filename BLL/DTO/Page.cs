using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class Page
    {
        public int NumberOfPages { get; set; }
        public IEnumerable<SlotMinimumDTO> Slots { get; set; }
    }
}
