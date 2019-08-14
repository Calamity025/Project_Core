using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;

namespace BLL.Services
{
    public class ProfileManagementService : IProfileManagementService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;

        public ProfileManagementService(IDataUnitOfWork db, IMapper mapper, IIdentityUnitOfWork identity)
        {
            _db = db;
            _mapper = mapper;
            _identity = identity;
        }

        public async Task CreateProfile(int userId, ProfileCreationDTO profile)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            user = _mapper.Map<UserInfo>(profile);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating profile", e);
            }
        }

        public async Task AddAvatarLink(int userId, string link)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            if (user == null)
            {
                throw new NotFoundException();
            }

            user.ImageLink = link;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when adding avatar link", e);
            }
        }
    }
}
