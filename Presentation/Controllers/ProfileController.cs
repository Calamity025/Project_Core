using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileManagementService _profileManagementService;
        private readonly IMapper _mapper;

        public ProfileController(IProfileManagementService profileManagementService, IMapper mapper)
        {
            _profileManagementService = profileManagementService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ProfileDTO> Get()
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                var profile = await _profileManagementService.GetProfile(id);
                Response.StatusCode = 200;
                return profile;
            }
            catch
            {
                Response.StatusCode = 404;
                return null;
            }
        }

        [HttpGet("followingSlots")]
        public async Task<int[]> GetFollowingMinimumSlots()
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                var slots = await _profileManagementService.GetFollowingSlots(id);
                var ids = slots.Select(x => x.Id).ToArray();
                if (ids.Any(x => x <= 0))
                {
                    Response.StatusCode = 400;
                    return null;
                }
                Response.StatusCode = 200;
                return ids;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
                return null;
            }
        }

        [HttpPost]
        public async Task Post([FromBody] ProfileCreationModel profile)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                await WriteErrors();
                return;
            }
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.CreateProfile(id, 
                    _mapper.Map<ProfileCreationDTO>(profile));
                Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }
        }

        [HttpPost]
        [Route("image")]
        public async Task Post(IFormFile file)
        {
            if (file == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("File is null");
                return;
            }
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\");
                }
                using (FileStream filestream = System.IO.File.Create(
                    Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\" + $"_{id}{Path.GetExtension(file.FileName)}"))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _profileManagementService.AddAvatarLink(id,
                        $"/UserAvatars/_{id}{Path.GetExtension(file.FileName)}");
                }
                Response.StatusCode = 201;
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }

        }

        [HttpPut("follow/{id}")]
        public async Task AddToFollowingList(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be null");
                return;
            }
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.AddToUserFollowingList(userId, id);
                Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }
        }

        [HttpPut("unfollow/{id}")]
        public async Task RemoveFromFollowingList(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Slot id cannot be null");
                return;
            }
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.RemoveFromUserFollowingList(userId, id);
                Response.StatusCode = 200;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
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
