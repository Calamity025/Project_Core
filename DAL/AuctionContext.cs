﻿using System.Threading.Tasks;
using DAL.Interfaces;
using Entities;
using Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AuctionContext : IdentityDbContext<User, Role, int>, IDbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options)
            : base(options) { }

        public DbSet<Slot> Slots { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Category> Categories { get; set; }

        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
