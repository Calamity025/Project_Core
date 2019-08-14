using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IProfileManagementService
    {
        Task CreateProfile(int userId, ProfileCreationDTO profile);
        Task AddAvatarLink(int userId, string link);
    }
}
