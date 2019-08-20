using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class SlotRepresentationService : ISlotRepresentationService
    {
        private readonly IDataUnitOfWork _db;
        private readonly IMapper _mapper;

        public SlotRepresentationService(IDataUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<Page> GetPage(int pageNumber, 
            int slotsOnPage)
        {
            var slotsTask = _db.Slots.GetAll()
                .Where(x => x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .OrderBy(x => x.Id).Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            var countTask = _db.Slots.GetAll()
                .Where(x => x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .CountAsync();
            var slots = await slotsTask;
            var count = await countTask;
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();
            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            int numberOfPages = 
                (count % slotsOnPage) > 0 ? (count / slotsOnPage + 1) : (count / slotsOnPage);
            return new Page(){NumberOfPages = numberOfPages, Slots = page};
        }

        public async Task<SlotFullDTO> GetSlot(int id)
        {
            var slot = await _db.Slots.GetAll().Where(x => x.Id == id)
                .Include(x => x.SlotTags)
                .ThenInclude(x => x.Tag)
                .Include(x=> x.Category)
                .FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new NotFoundException();
            }
            var user = await _db.UserInfos.GetAsync(slot.UserInfoId);
            if (user == null)
            {
                throw new NotFoundException();
            }
            var history = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == id)
                .OrderByDescending(x => x.Price)
                .FirstOrDefaultAsync();
            var slotDTO = _mapper.Map<SlotFullDTO>(slot);
            slotDTO.Price = history?.Price;
            slotDTO.User = new UserDTO
            {
                Id = user.Id,
                AvatarLink = user.ImageLink,
                Name = (!string.IsNullOrEmpty(user.LastName) && !string.IsNullOrEmpty(user.FirstName)) ?
                    (user.LastName + " " + user.FirstName) : "anonymous"
            };
            return slotDTO;
        }

        public async Task<decimal> GetSlotPrice(int id)
        {
            var slot = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == id)
                .OrderByDescending(x => x.Price)
                .FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new NotFoundException();
            }
            return slot.Price;
        }

        public async Task<decimal> GetUserBet(int id, int userId)
        {
            var slot = await _db.BetHistories.GetAll()
                .Where(x => x.Slot.Id == id && x.BetUserInfoId == userId)
                .OrderByDescending(x => x.Price )
                .FirstOrDefaultAsync();
            if (slot == null)
            {
                throw new NotFoundException();
            }
            return slot.Price;
        }

        public async Task<Page> GetByCategory(int categoryId, 
            int pageNumber, int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll()
                .Where(x => x.CategoryId.Equals(categoryId) && 
                            x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .OrderBy(x => x.Id).Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            var count = await _db.Slots.GetAll()
                .Where(x => x.CategoryId.Equals(categoryId) && 
                            x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .CountAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();
            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            int numberOfPages = 
                (count % slotsOnPage) > 0 ? (count / slotsOnPage + 1) : (count / slotsOnPage);
            return new Page(){NumberOfPages = numberOfPages, Slots = page};
        }

        public async Task<Page> GetByTags(IEnumerable<int> tagIds, int pageNumber,
            int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll()
                .Where(slot => tagIds.All(id => slot.SlotTags.Any(tag => tag.TagId == id)) && 
                                slot.Status.Equals(Status.SlotStatus.Started.ToString()))
                .OrderBy(x => x.Id).Skip(--pageNumber * slotsOnPage).Take(slotsOnPage)
                .ToListAsync();
            var count = await _db.Slots.GetAll()
                .Where(slot => tagIds.All(id => slot.SlotTags.Any(tag => tag.TagId == id)) && 
                                slot.Status.Equals(Status.SlotStatus.Started.ToString()))
                .CountAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();
            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            int numberOfPages = 
                (count % slotsOnPage) > 0 ? (count / slotsOnPage + 1) : (count / slotsOnPage);
            return new Page() { NumberOfPages = numberOfPages, Slots = page };
        }

        public async Task<Page> GetByName(string query, 
            int pageNumber, int slotsOnPage)
        {
            IEnumerable<Slot> slots = await _db.Slots.GetAll()
                .Where(x => x.Name.Contains(query) && x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .OrderBy(x => x.Id).Skip(--pageNumber * slotsOnPage).Take(slotsOnPage).ToListAsync();
            var count = await _db.Slots.GetAll()
                .Where(x => x.Name.Contains(query) && x.Status.Equals(Status.SlotStatus.Started.ToString()))
                .CountAsync();
            List<SlotMinimumDTO> page = new List<SlotMinimumDTO>();
            foreach (var slot in slots)
            {
                page.Add(_mapper.Map<SlotMinimumDTO>(slot));
            }
            int numberOfPages = 
                (count % slotsOnPage) > 0 ? (count / slotsOnPage + 1) : (count / slotsOnPage);
            return new Page() { NumberOfPages = numberOfPages, Slots = page };
        }

        public async Task<IEnumerable<Slot>> GetExpiredSlots()
        {
            return await _db.Slots.GetAll()
                .Where(x => x.EndTime <= DateTime.Now && !x.Status.Equals("finished")).ToListAsync();
        }
    }
}
