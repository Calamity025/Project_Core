using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IMapper _mapper;

        public CategoryManagementService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetCategoryList()
        {
            return await _db.Categories.GetAll().ToListAsync();
        }

        public async Task<Category> CreateCategory(Category category)
        {
            _db.Categories.Create(category);

            try
            {
                await _db.SaveChangesAsync();
                return category;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating a category", e);
            }
        }

        public async Task<Category> UpdateCategory(Category newCategory)
        {
            var category = await _db.Categories.GetAsync(newCategory.Id);
            category.Name = newCategory.Name;
            _db.Update(category);

            try
            {
                await _db.SaveChangesAsync();
                return category;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating a category", e);
            }
        }

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _db.Dispose();
            }

            disposed = true;
        }

        ~CategoryManagementService()
        {
            Dispose(false);
        }
    }
}
