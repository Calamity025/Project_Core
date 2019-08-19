using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;

namespace DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly IDbContext _context;

        public TagRepository(IDbContext context) =>
            _context = context;

        public IQueryable<Tag> GetAll() =>
            _context.Tags;

        public Tag Get(int id) =>
            _context.Tags.Find(id);

        public async Task<Tag> GetAsync(int id) =>
            await _context.Tags.FindAsync(id);

        public void Create(Tag item) =>
            _context.Tags.Add(item);

        public void Delete(Tag tag) =>
            _context.Tags.Remove(tag);
    }
}
