using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class TagManagementService : ITagManagementService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IMapper _mapper;

        public TagManagementService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Tag>> GetTagList()
        {
            return await _db.Tags.GetAll().ToListAsync();
        }

        public async Task<Tag> CreateTag(Tag tag)
        {
            _db.Tags.Create(tag);

            try
            {
                await _db.SaveChangesAsync();
                return tag;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when creating a tag", e);
            }
        }

        public async Task<Tag> UpdateTag(Tag newTag)
        {
            var tag = await _db.Tags.GetAsync(newTag.Id);
            tag.Name = newTag.Name;
            _db.Update(tag);

            try
            {
                await _db.SaveChangesAsync();
                return tag;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when updating a tag", e);
            }
        }


        public async Task<Tag> DeleteTag(int id)
        {
            var tag = await _db.Tags.GetAsync(id);
            _db.Tags.Delete(tag);

            try
            {
                await _db.SaveChangesAsync();
                return tag;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error when deleting a tag", e);
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

        ~TagManagementService()
        {
            Dispose(false);
        }
    }
}
