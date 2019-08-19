using System;
using Entities;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        UserManager<User> UserManager { get; }
        RoleManager<Role> RoleManager { get; }
    }
}
