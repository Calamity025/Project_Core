using System.Linq;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T Get(int id);
        Task<T> GetAsync(int id);
        void Create(T item);
        void Delete(T item);
    }
}
