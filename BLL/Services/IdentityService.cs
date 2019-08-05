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
        private readonly IIdentityUnitOfWork _identity;
        private readonly IMapper _mapper;

        public IdentityService(IIdentityUnitOfWork identity, IMapper mapper)
        {
            _identity = identity;
            _mapper = mapper;
        }

        public async Task Register(UserCreationDTO userCreation)
        {
            User user = _mapper.Map<User>(userCreation);
            await _identity.UserManager.CreateAsync(user, userCreation.Password);
            await _identity.UserManager.AddClaimAsync(user,
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
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
    }
}
