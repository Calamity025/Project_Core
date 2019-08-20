using System;
using AutoMapper;
using BLL;
using BLL.Services;
using DAL;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BLLTests
{
    [TestFixture]
    public class IdentityServiceTests : IDisposable
    {
        private static readonly IdentityService _identityService;
        private static readonly AuctionContext _context;
        static IdentityServiceTests()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder();
            options.UseSqlServer("Server=.;Database=Auction;Trusted_Connection=True;");
            _context = new AuctionContext(options.Options);
            var db = new DataUnitOfWork(context);
            var identity = new IdentityUnitOfWork();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BLLProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _identityService = new IdentityService(db, );
        }

        [Test]


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
                _db.Dispose();
            }
            disposed = true;
        }

        ~IdentityServiceTests() =>
            Dispose(false);
    }
}
