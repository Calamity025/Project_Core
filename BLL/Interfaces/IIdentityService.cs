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
        Task<UserDTO> Register(IdentityCreationDTO identity);
        Task<ClaimsIdentity> Login(LoginInfo info, string authType);
        Task<UserDTO> GetCurrentUser(string name);
        Task DeleteIdentity(string name);
        Task AddToRoleAsync(string userName, string role);
    }
}
