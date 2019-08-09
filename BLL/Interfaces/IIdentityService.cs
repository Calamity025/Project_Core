using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using Entities;

namespace BLL.Interfaces
{
    public interface IIdentityService : IDisposable
    {
        Task Register(UserCreationDTO user);
        Task<ClaimsIdentity> Login(UserLoginInfo info, string authType);
        Task<UserLoginResponse> GetCurrentUser(string name);
    }
}
