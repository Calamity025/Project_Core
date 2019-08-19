using System.Collections.Generic;

namespace BLL.DTO
{
    public class Page
    {
        public int NumberOfPages { get; set; }
        public IEnumerable<SlotMinimumDTO> Slots { get; set; }
    }
}
