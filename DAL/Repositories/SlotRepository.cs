using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;

namespace DAL.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IDbContext _context;

        public SlotRepository(IDbContext context) =>
            _context = context;

        public IQueryable<Slot> GetAll() =>
            _context.Slots;

        public Slot Get(int id) =>
            _context.Slots.Find(id);

        public async Task<Slot> GetAsync(int id) =>
           await _context.Slots.FindAsync(id);

        public void Create(Slot item) =>
            _context.Slots.Add(item);

        public void Delete(Slot slot) =>
            _context.Slots.Remove(slot);

        public void CreateSlotTag(SlotTag item) =>
            _context.SlotTags.Add(item);

        public void DeleteSlotTag(SlotTag item) =>
            _context.SlotTags.Remove(item);
    }
}
