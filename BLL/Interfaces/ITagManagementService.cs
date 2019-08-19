using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace BLL.Interfaces
{
    public interface ITagManagementService
    {
        Task<IEnumerable<Tag>> GetTagList();
        Task CreateTag(string tagName);
        Task UpdateTag(int id, string newTagName);
        Task DeleteTag(int id);
    }
}
