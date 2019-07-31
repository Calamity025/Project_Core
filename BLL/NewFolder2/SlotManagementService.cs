
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL;
using DAL.Interfaces;
using Entities;

public class SlotManagementService : ISlotManagementService
{
    private readonly IDataUnitOfWork _db;
    private readonly IMapper _mapper;

    public SlotManagementService(IDataUnitOfWork db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task CreateSlot(int userId, SlotCreationDTO slotDTO)
    {
        UserInfo seller = await _db.UserInfos.GetAsync(userId);
        Category category = await _db.Categories.GetAsync(slotDTO.CategoryId);
        if (seller == null || category == null)
        {
            throw new NotFoundException();
        }

        List<Tag> tags = await _db.Tags.GetAll().Where(x => slotDTO.SlotTagsId.Contains(x.Id)).ToListAsync();
        var newSlot = _mapper.Map<Slot>(slotDTO);
        newSlot.Category = category;
        newSlot.SlotTags = tags;
        _db.Slots.Create(newSlot);
        
        seller.PlacedSlots.Add(newSlot);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when creating slot", e);
        }
    }

    public async Task DeleteSlot(int id)
    {
        _db.Slots.Delete(id);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when deleting slot", e);
        }
    }

    public async Task AddTags(int slotId, IEnumerable<int> tagIds)
    {
        Slot slot = await _db.Slots.GetAsync(slotId);
        if (slot == null)
        {
            throw new NotFoundException();
        }

        ICollection<Tag> tags = await _db.Tags.GetAll().Where(x => tagIds.Contains(x.Id)).ToListAsync();
        slot.SlotTags = slot.SlotTags.Concat(tags);
        _db.Update(slot);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when updating slot", e);
        }
    }

    public async Task RemoveTags(int slotId, IEnumerable<int> tagIds)
    {
        Slot slot = await _db.Slots.GetAsync(slotId);
        if (slot == null)
        {
            throw new NotFoundException();
        }

        List<Tag> tags = slot.SlotTags.Where(x => !tagIds.Contains(x.Id)).ToListAsync();

        slot.SlotTags = tags;
        _db.Update(slot);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when updating slot", e);
        }
    }

    public async Task UpdateGeneralInfo(int slotId, SlotGeneralInfoDTO slotInfo)
    {
        Slot slot = await _db.Slots.GetAsync(slotId);
        if (slot == null)
        {
            throw new NotFoundException();
        }

        slot.Name = slotInfo.Name;
        slot.Description = slot.Description;

        _db.Update(slot);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when updating slot", e);
        }
    }

    public async Task UpdateStatus(int slotId, Status status)
    {
        Slot slot = await _db.Slots.GetAsync(slotId);
        if (slot == null)
        {
            throw new NotFoundException();
        }

        slot.Status = status.ToString();
        _db.Update(slot);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when updating slot", e);
        }
    }

    public async Task AddToUsersFollowingList(int userId, int slotId)
    {
        UserInfo user = await _db.UserInfos.GetAsync(userId);
        Slot slot = await _db.Slots.GetAsync(slotId);
        if (user == null || slot == null) 
        {
            throw new NotFoundException();
        }

        user.FollowingSlots.Add(slot);

        _db.Update(user);
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new DatabaseException("Error when updating", e);
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

    ~SlotManagementService()
    {
        Dispose(false);
    }
}
