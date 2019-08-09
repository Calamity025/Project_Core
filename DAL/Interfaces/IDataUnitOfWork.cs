﻿using System;
using System.Threading.Tasks;
using DAL.Repositories;
using Entities;

namespace DAL.Interfaces
{
    public interface IDataUnitOfWork : IDisposable
    {
        ISlotRepository Slots { get; }
        IUserInfoRepository UserInfos { get; }
        ITagRepository Tags { get; }
        ICategoryRepository Categories { get; }
        IBetHistoryRepository BetHistories { get; }
        void Update(object item);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
