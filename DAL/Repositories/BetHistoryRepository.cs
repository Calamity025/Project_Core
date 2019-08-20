using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;

namespace DAL.Repositories
{
    public class BetHistoryRepository : IBetHistoryRepository
    {
        private readonly IDbContext _context;

        public BetHistoryRepository(IDbContext context) =>
            _context = context;

        public IQueryable<BetHistory> GetAll() =>
            _context.BetHistories;

        public BetHistory Get(int id) =>
            _context.BetHistories.Find(id);

        public async Task<BetHistory> GetAsync(int id) =>
            await _context.BetHistories.FindAsync(id);

        public void Create(BetHistory item) =>
            _context.BetHistories.Add(item);

        public void Delete(BetHistory category) =>
            _context.BetHistories.Remove(category);

        public void DeleteRange(IEnumerable<BetHistory> categories) =>
            _context.BetHistories.RemoveRange(categories);
    }
}
