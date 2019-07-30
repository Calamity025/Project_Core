using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DAL.Interfaces
{
    public interface IIdentityUnitOfWork : IDisposable
    {
        UserManager<User> UserManager { get; }
        RoleManager<Role> RoleManager { get; }
    }
}
