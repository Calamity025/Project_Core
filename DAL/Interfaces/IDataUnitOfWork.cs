using System;
using System.Threading.Tasks;
using Entities;

namespace DAL.Interfaces
{
    public interface IDataUnitOfWork : IDisposable
    {
        ISlotRepository Slots { get; }
        IUserRepository UserInfos { get; }
        ITagRepository Tags { get; }
        ICategoryRepository Categories { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
