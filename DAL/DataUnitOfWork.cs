using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataUnitOfWork : IDataUnitOfWork
    {
        private IDbContext _context;
        private ISlotRepository _slotRepository;
        private IUserInfoRepository _userInfoRepository;
        private ITagRepository _tagRepository;
        private ICategoryRepository _categoryRepository;
        private IBetHistoryRepository _betHistoryRepository;

        public DataUnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public ISlotRepository Slots => 
            _slotRepository ?? (_slotRepository = new SlotRepository(_context));
        public IUserInfoRepository UserInfos =>
            _userInfoRepository ?? (_userInfoRepository = new UserInfoRepository(_context));
        public ITagRepository Tags => 
            _tagRepository ?? (_tagRepository = new TagRepository(_context));
        public ICategoryRepository Categories => 
            _categoryRepository ?? (_categoryRepository = new CategoryRepository(_context));
        public IBetHistoryRepository BetHistories =>
            _betHistoryRepository ?? (_betHistoryRepository = new BetHistoryRepository(_context));

        public void Update(object item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        bool disposed = false;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }

            disposed = true;
        }

        ~DataUnitOfWork()
        {
            Dispose(false);
        }
    }
}
