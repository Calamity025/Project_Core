using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AccountController(IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("/Register")]
        [AllowAnonymous]
        public async Task Register([FromBody] UserRegistrationModel userInfo)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(ModelState.ToString());
                return;
            }

                var user = await _identityService.Register(_mapper.Map<IdentityCreationDTO>(userInfo));
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonConvert.SerializeObject(user,
                    new JsonSerializerSettings {Formatting = Formatting.Indented}));
            
        }

        [AllowAnonymous]
        [HttpPost("/token")]
        public async Task Token(LoginInfo info)
        {
            var identity = await _identityService.Login(info, "Token");
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                claims = identity.Claims.First(x=> x.Type == "Id").Value
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Current")]
        public async Task GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.StatusCode = 401;
                return;
            }

            CurrentUserModel response = _mapper.Map<CurrentUserModel>(await _identityService.GetCurrentUser(User.Identity.Name));
            response.isAuthorized = User.Identity.IsAuthenticated;

            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }
    }
}