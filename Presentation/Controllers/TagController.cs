using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagManagementService _tagManagementService;

        public TagController(ITagManagementService tagManagementService)
        {
            _tagManagementService = tagManagementService;
        }

        // GET: api/Tag
        [HttpGet]
        public async Task<IEnumerable<Tag>> Get()
        {
            return await _tagManagementService.GetTagList();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<Tag> Post([FromBody] Tag tag)
        {
            try
            {
                return await _tagManagementService.CreateTag(tag);
            }
            catch
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<Tag> Put(int id, [FromBody] Tag tag)
        {
            tag.Id = id;
            try
            {
                return await _tagManagementService.UpdateTag(tag);
            }
            catch
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            try
            {
                await _tagManagementService.DeleteTag(id);
                Response.StatusCode = 204;
            }
            catch
            {
                Response.StatusCode = 500;
            }
        }
    }
}
