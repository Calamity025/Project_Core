using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class UserInfoRepository : IUserRepository
    {
        private IDbContext _context;

        public UserInfoRepository(IDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserInfo> GetAll()
        {
            return _context.UserInfos;
        }

        public UserInfo Get(int id)
        {
            return _context.UserInfos.Find(id);
        }

        public async Task<UserInfo> GetAsync(int id)
        {
            return await _context.UserInfos.FindAsync(id);
        }

        public void Create(UserInfo item)
        {
            _context.UserInfos.Add(item);
        }

        public void Delete(UserInfo info)
        {
           _context.UserInfos.Remove(info);
        }
    }
}
