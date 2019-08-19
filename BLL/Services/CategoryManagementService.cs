using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IDataUnitOfWork _db;

        public CategoryManagementService(IDataUnitOfWork db) =>
            _db = db;

        public async Task<IEnumerable<Category>> GetCategoryList() =>
            await _db.Categories.GetAll().ToListAsync();

        public async Task CreateCategory(string categoryName)
        {
            var category = new Category() {Name = categoryName};
            _db.Categories.Create(category);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating a category", e);
            }
        }

        public async Task UpdateCategory(int id, string newCategoryName)
        {
            var category = await _db.Categories.GetAsync(id);
            category.Name = newCategoryName;
            _db.Update(category);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating a category", e);
            }
        }
    }
}
