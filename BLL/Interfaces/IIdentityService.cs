using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IIdentityService
    {
        Task Register(UserCreationDTO user);
        Task<ClaimsIdentity> Login(UserLoginInfo info, string authType);
    }
}
