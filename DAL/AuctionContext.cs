using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL
{
    public class AuctionContext : IdentityDbContext<User, Role, int>, IDbContext
    {
        public AuctionContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Slot> Slots { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BetHistory> BetHistories { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Slot>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Entity<Tag>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Entity<Category>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Entity<User>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Entity<Role>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
            builder.Entity<BetHistory>()
                .Property(p => p.Id)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
