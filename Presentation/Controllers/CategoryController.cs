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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManagementService _categoryManagementService;

        public CategoryController(ICategoryManagementService categoryManagementService) =>
            _categoryManagementService = categoryManagementService;

        [HttpGet]
        public async Task<IEnumerable<Category>> Get() =>
            await _categoryManagementService.GetCategoryList();

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task Post([FromBody] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName) || categoryName.Length > 25)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Category name should be between 0 and 25 characters");
                return;
            }
            try
            {
                await _categoryManagementService.CreateCategory(categoryName);
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
        public async Task Put(int id, [FromBody] string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName) || categoryName.Length > 25)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Category name should be between 0 and 25 characters");
                return;
            }
            try
            {
                await _categoryManagementService.UpdateCategory(id, categoryName);
                Response.StatusCode = 204;
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
