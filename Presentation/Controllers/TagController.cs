using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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

        public TagController(ITagManagementService tagManagementService) =>
            _tagManagementService = tagManagementService;

        [HttpGet]
        public async Task<IEnumerable<Tag>> Get() =>
            await _tagManagementService.GetTagList();

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task Post([FromBody] string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || tagName.Length > 25)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Tag name should be between 0 and 25 characters");
                return;
            }
            try
            {
                await _tagManagementService.CreateTag(tagName);
                Response.StatusCode = 201;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || tagName.Length > 25)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Tag name should be between 0 and 25 characters");
                return;
            }
            try
            {
                await _tagManagementService.UpdateTag(id, tagName);
                Response.StatusCode = 204;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (id <= 0)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Tag id cannot be 0 or lower");
                return;
            }
            try
            {
                await _tagManagementService.DeleteTag(id);
                Response.StatusCode = 204;
            }
            catch (Exception e)
            {
                Response.StatusCode = 500;
                await Response.WriteAsync(e.ToString());
            }
        }
    }
}
