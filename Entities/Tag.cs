
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<SlotTag> Slots { get; } = new List<SlotTag>();
    }
}
