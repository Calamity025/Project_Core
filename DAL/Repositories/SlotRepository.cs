using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly IDbContext _context;

        public SlotRepository(IDbContext context)
        {
            _context = context;
        }

        public IQueryable<Slot> GetAll()
        {
            return _context.Slots;
        }

        public Slot Get(int id)
        {
            return _context.Slots.Find(id);
        }

        public async Task<Slot> GetAsync(int id)
        {
            return await _context.Slots.FindAsync(id);
        }
        
        public void Create(Slot item)
        {
            _context.Slots.Add(item);
        }

        public void Delete(Slot slot)
        {
            _context.Slots.Remove(slot);
        }
    }
}
