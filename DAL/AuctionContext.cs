using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
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
        public DbSet<SlotTag> SlotTags { get; set; }
        public DbSet<FollowingSlots> FollowingSlots { get; set; }

        public Task<int> SaveChangesAsync() => 
            base.SaveChangesAsync();

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            builder.Entity<SlotTag>()
                .HasKey(x => new {x.SlotId, x.TagId});
            builder.Entity<SlotTag>()
                .HasOne(x => x.Slot)
                .WithMany(x => x.SlotTags)
                .HasForeignKey(x => x.SlotId);
            builder.Entity<SlotTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.Slots)
                .HasForeignKey(x => x.TagId);
            builder.Entity<FollowingSlots>()
                .HasKey(x => new {x.SlotId, x.FollowingUserInfoId});
            builder.Entity<FollowingSlots>()
                .HasOne(x => x.Slot)
                .WithMany(x => x.Following)
                .HasForeignKey(x => x.SlotId);
            builder.Entity<FollowingSlots>()
                .HasOne(x => x.FollowingUserInfo)
                .WithMany(x => x.FollowingSlots)
                .HasForeignKey(x => x.FollowingUserInfoId);
            builder.Entity<BetHistory>()
                .HasKey(x => new { x.SlotId, x.BetUserInfoId});
            builder.Entity<BetHistory>()
                .HasOne(x => x.Slot)
                .WithMany(x => x.Betters)
                .HasForeignKey(x => x.SlotId);
            builder.Entity<BetHistory>()
                .HasOne(x => x.BetUserInfo)
                .WithMany(x => x.BetSlots)
                .HasForeignKey(x => x.BetUserInfoId);
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
            builder.Entity<Slot>()
                .HasOne(x => x.UserInfo)
                .WithMany(x => x.PlacedSlots)
                .HasForeignKey(x => x.UserInfoId);
        }
    }
}
