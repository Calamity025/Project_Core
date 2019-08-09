using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManagementService _categoryManagementService;

        public CategoryController(ICategoryManagementService categoryManagementService)
        {
            _categoryManagementService = categoryManagementService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            return await _categoryManagementService.GetCategoryList();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<Category> Post([FromBody] Category category)
        {
            try
            {
                return await _categoryManagementService.CreateCategory(category);
            }
            catch
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<Category> Put(int id, [FromBody] Category category)
        {
            category.Id = id;
            try
            {
                return await _categoryManagementService.UpdateCategory(category);
            }
            catch
            {
                Response.StatusCode = 500;
                return null;
            }
        }
    }
}
