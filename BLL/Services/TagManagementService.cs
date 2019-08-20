using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TagManagementService : ITagManagementService
    {
        private readonly IDataUnitOfWork _db;

        public TagManagementService(IDataUnitOfWork db) =>
            _db = db;

        public async Task<IEnumerable<Tag>> GetTagList() =>
            await _db.Tags.GetAll().OrderBy(x => x.Name).ToListAsync();

        public async Task CreateTag(string tagName)
        {
            var tag = new Tag(){Name = tagName};
            _db.Tags.Create(tag);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating a tag", e);
            }
        }

        public async Task UpdateTag(int id, string newTagName)
        {
            var tag = await _db.Tags.GetAsync(id);
            tag.Name = newTagName;
            _db.Update(tag);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating a tag", e);
            }
        }


        public async Task DeleteTag(int id)
        {
            var tag = await _db.Tags.GetAsync(id);
            _db.Tags.Delete(tag);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when deleting a tag", e);
            }
        }
    }
}
