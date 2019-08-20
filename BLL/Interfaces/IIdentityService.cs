using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IIdentityService
    {
        Task Register(IdentityCreationDTO identity);
        Task<ClaimsIdentity> Login(LoginInfoDTO infoDto, string authType);
        Task<UserDTO> GetCurrentUser(string name);
        Task DeleteIdentity(string name);
        Task AddToRole(string userName, string role);
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<decimal> AddMoney(int userId, decimal value);
    }
}
