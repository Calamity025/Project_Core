using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private IDbContext _context;

        public TagRepository(IDbContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> GetAll()
        {
            return _context.Tags;
        }

        public Tag Get(int id)
        {
            return _context.Tags.Find(id);
        }

        public async Task<Tag> GetAsync(int id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public void Create(Tag item)
        {
            _context.Tags.Add(item);
        }

        public void Delete(Tag tag)
        { 
            _context.Tags.Remove(tag);
        }
    }
}
