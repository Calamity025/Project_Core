using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.DTO;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileManagementService _profileManagementService;

        public ProfileController(IProfileManagementService profileManagementService)
        {
            _profileManagementService = profileManagementService;
        }
        // GET: api/Profile
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Profile/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [Authorize]
        [HttpGet("followingSlots")]
        public async Task<int[]> GetFollowingMinimumSlots()
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                var slots = await _profileManagementService.GetFollowingSlots(id);
                Response.StatusCode = 200;
                return slots.Select(x => x.Id).ToArray();
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
                return null;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return null;
            }

        }

        // POST: api/Profile
        [Authorize]
        [HttpPost]
        public async Task Post([FromBody] ProfileCreationDTO profile)
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.CreateProfile(id, profile);
                Response.StatusCode = 200;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("image")]
        public async Task Post(IFormFile file)
        {
            var id = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\");
                }

                using (FileStream filestream = System.IO.File.Create(
                    Directory.GetCurrentDirectory() + "\\wwwroot\\UserAvatars\\" +
                    $"{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}"))
                {
                    file.CopyTo(filestream);
                    filestream.Flush();
                    await _profileManagementService.AddAvatarLink(id,
                        $"/UserAvatars/{Path.GetFileNameWithoutExtension(file.FileName)}_{id}{Path.GetExtension(file.FileName)}");
                }

                Response.StatusCode = 200;
                await Response.WriteAsync(id.ToString());
            }
            catch (NotFoundException)
            {
                Response.StatusCode = 404;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
            }

        }

        // PUT: api/Profile/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [Authorize]
        [HttpPut("follow/{id}")]
        public async Task AddToFollowingList(int id)
        {
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.AddToUserFollowingList(userId, id);
                Response.StatusCode = 200;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        [Authorize]
        [HttpPut("unfollow/{id}")]
        public async Task RemoveFromFollowingList(int id)
        {
            var userId = Convert.ToInt32(User.Claims.First(x => x.Type == "Id").Value);
            try
            {
                await _profileManagementService.RemoveFromUserFollowingList(userId, id);
                Response.StatusCode = 200;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
