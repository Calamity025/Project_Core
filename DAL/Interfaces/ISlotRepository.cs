using System.Collections.Generic;
using Entities;

namespace DAL.Interfaces
{
    public interface ISlotRepository : IRepository<Slot>
    {
        void CreateSlotTag(SlotTag item);
        void DeleteSlotTag(SlotTag item);
    }
}
