using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace BLL.Interfaces
{
    public interface ICategoryManagementService
    {
        Task<IEnumerable<Category>> GetCategoryList();
        Task CreateCategory(string categoryName);
        Task UpdateCategory(int id, string newCategoryName);
    }
}
