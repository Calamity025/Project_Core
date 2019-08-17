using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (!TryValidateModel(userInfo))
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(ModelState.ToString());
                return;
            }

            try
            {
                var user = await _identityService.Register(_mapper.Map<IdentityCreationDTO>(userInfo));
                Response.StatusCode = 200;
                await Response.WriteAsync(JsonConvert.SerializeObject(user,
                    new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            catch
            {
                Response.StatusCode = 500;
            }
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
                claims = identity.Claims.FirstOrDefault(x=> x.Type == "Id")?.Value
            };
            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }

        [HttpGet]
        [Route("Current")]
        public async Task GetCurrentUser()
        {
            UserDTO response = await _identityService.GetCurrentUser(User.Identity.Name);

            await Response.WriteAsync(JsonConvert.SerializeObject(response,
                new JsonSerializerSettings {Formatting = Formatting.Indented}));
        }
    }
}