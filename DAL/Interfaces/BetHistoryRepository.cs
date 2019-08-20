using System.Collections.Generic;
using Entities;

namespace DAL.Interfaces
{
    public interface IBetHistoryRepository : IRepository<BetHistory>
    {
        void DeleteRange(IEnumerable<BetHistory> categories);
    }
}
