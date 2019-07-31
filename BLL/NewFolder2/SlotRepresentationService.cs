using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Interfaces;
using Entities;

namespace BLL
{
    class SlotRepresentationService : ISlotRepresentationService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IMapper _mapper;

        public SlotRepresentationService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetPage(int pageNumber, 
            int slotsOnPage)
        {
            var slots = await _db.Slots.GetAll().Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();
            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }

            return page;
        }

        public async Task<SlotFullDTO> GetSlot(int id)
        {
            Slot slot = await _db.Slots.GetAsync(id);
            if (slot == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<SlotFullDTO>(slot);
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetByCategory(int categoryId, 
            int pageNumber, int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll().Where(x => x.CategoryId.Equals(categoryId))
                .Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();

            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }

            return page;
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetByTags(IEnumerable<int> tagIds, int pageNumber,
            int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll().Where(x => tagIds.Contains(x.SlotTags)) //TODO make smth
                .Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();

            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }

            return page;
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetByName(string query, 
            int pageNumber, int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll().Where(x => x.Name.Contains(query)) 
                .Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();

            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }

            return page;
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetUserFollowingSlots(int userId)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            List<SlotMinimumDTO> slots = new List<SlotMinimumDTO>();

            foreach (var userFollowingSlot in user.FollowingSlots)
            {
                slots.Add(_mapper.Map<SlotMinimumDTO>(userFollowingSlot));
            }

            return slots;
        }

        public async Task<IEnumerable<SlotMinimumDTO>> GetUserSlots(int userId)
        {
            UserInfo user = await _db.UserInfos.GetAsync(userId);
            List<SlotMinimumDTO> slots = new List<SlotMinimumDTO>();

            foreach (var userSlot in user.PlacedSlots) 
            {
                slots.Add(_mapper.Map<SlotMinimumDTO>(userSlot));
            }

            return slots;
        }

        public FileStream GetSlotImage(int id) 
        {
            return _db.ImageRepository.GetSlotImage(id);
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

        ~SlotRepresentationService()
        {
            Dispose(false);
        }
    }
}
