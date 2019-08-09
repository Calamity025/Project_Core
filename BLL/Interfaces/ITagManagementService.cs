using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace BLL.Interfaces
{
    public interface ITagManagementService : IDisposable
    {
        Task<IEnumerable<Tag>> GetTagList();
        Task<Tag> CreateTag(Tag tag);
        Task<Tag> UpdateTag(Tag newTag);
        Task<Tag> DeleteTag(int id);
    }
}
