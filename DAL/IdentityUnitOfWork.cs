﻿using System;
using DAL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public IdentityUnitOfWork(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public UserManager<User> UserManager => _userManager;
        public RoleManager<Role> RoleManager => _roleManager;
        
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
                _userManager.Dispose();
                _roleManager.Dispose();
            }
            disposed = true;
        }

        ~IdentityUnitOfWork() =>
            Dispose(false);
    }
}
