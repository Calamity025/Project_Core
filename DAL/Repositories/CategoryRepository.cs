using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;

namespace DAL.Repositories
{
    class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContext _context;

        public CategoryRepository(IDbContext context) =>
            _context = context;

        public IQueryable<Category> GetAll() =>
            _context.Categories;

        public Category Get(int id) =>
            _context.Categories.Find(id);

        public async Task<Category> GetAsync(int id) =>
            await _context.Categories.FindAsync(id);

        public void Create(Category item) =>
            _context.Categories.Add(item);

        public void Delete(Category category) =>
            _context.Categories.Remove(category);
    }
}
