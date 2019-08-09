using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace BLL.Interfaces
{
    public interface ICategoryManagementService : IDisposable
    {
        Task<IEnumerable<Category>> GetCategoryList();
        Task<Category> CreateCategory(Category category);
        Task<Category> UpdateCategory(Category newCategory);
    }
}
