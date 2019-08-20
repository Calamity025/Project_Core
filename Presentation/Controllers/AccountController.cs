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
                await WriteErrors();
                return;
            }
                var info = _mapper.Map<IdentityCreationDTO>(userInfo);
                await _identityService.Register(info);
                Response.StatusCode = 201;
            
        }

        [AllowAnonymous]
        [HttpPost("/token")]
        public async Task Token(LoginInfoModel infoModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                await WriteErrors();
                return;
            }
            var infoDTO = _mapper.Map<LoginInfoDTO>(infoModel);
            var identity = await _identityService.Login(infoDTO, "Token");
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
                access_token = encodedJwt
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }

        [HttpGet]
        [Route("Current")]
        public async Task<UserDTO> GetCurrentUser()
        {
            var response = await _identityService.GetCurrentUser(User.Identity.Name);
            Response.StatusCode = 200;
            return response;
        }

        [Authorize]
        [HttpDelete]
        public async Task Delete() => 
            await _identityService.DeleteIdentity(User.Identity.Name);

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers() =>
            await _identityService.GetUsers();

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task AddToRoleAdmin([FromBody] string userName)
        {
            if (string.IsNullOrEmpty(userName) || userName.Length > 15)
            {
                Response.StatusCode = 400;
                await WriteErrors();
                return;
            }
            try
            {
                await _identityService.AddToRole(userName, "admin");
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize]
        [HttpPut("addMoney")]
        public async Task<decimal?> AddMoney([FromBody] decimal value)
        {
            if (value <= 0)
            {
                Response.StatusCode = 400;
                return null;
            }
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                return await _identityService.AddMoney(userId, value);
            }
            catch
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        private async Task WriteErrors()
        {
            foreach (var modelStateValue in ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    await Response.WriteAsync(error.ErrorMessage);
                }
            }
        }
    }
}