using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Identity;
using UserLoginInfo = BLL.DTO.UserLoginInfo;

namespace BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;

        public IdentityService(IDataUnitOfWork db, IIdentityUnitOfWork identity, IMapper mapper)
        {
            _db = db;
            _identity = identity;
            _mapper = mapper;
        }

        public async Task Register(UserCreationDTO userCreation)
        {
            User user = _mapper.Map<User>(userCreation);
            UserInfo info = new UserInfo(){User = user, Id = user.Id};
            _db.UserInfos.Create(info);
            user.UserInfo = info;
            try
            {
                await _identity.UserManager.CreateAsync(user, userCreation.Password);
                await _identity.UserManager.AddClaimAsync(user, new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
                await _identity.UserManager.AddClaimAsync(user, new Claim("Id", user.Id.ToString()));
            }
            catch (Exception e)
            {
                _db.UserInfos.Delete(info);
                await _db.SaveChangesAsync();
                throw new DatabaseException("Error when creating user", e);
            }
        }

        public async Task<ClaimsIdentity> Login(UserLoginInfo loginInfo, string authType)
        {
            User user = await _identity.UserManager.FindByNameAsync(loginInfo.Login);
            if (user != null && await _identity.UserManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                var claims = await _identity.UserManager.GetClaimsAsync(user);

                return new ClaimsIdentity(claims, authType);
            }
            return null;
        }

        public async Task<UserLoginResponse> GetCurrentUser(string name)
        {
            User user = await _identity.UserManager.FindByNameAsync(name);
            UserLoginResponse userInfo = new UserLoginResponse()
            {
                ClaimsIdentity = new ClaimsIdentity(await _identity.UserManager.GetClaimsAsync(user), "Token"),
                User = _mapper.Map<UserLoginResponse.UserDTO>(user)
            };
            UserInfo info = await _db.UserInfos.GetAsync(user.Id);
            userInfo.User.AvatarLink = info.ImageLink;
            return userInfo;
        }

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
                _identity.Dispose();
            }

            disposed = true;
        }

        ~IdentityService()
        {
            Dispose(false);
        }
    }
}
