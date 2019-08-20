using System.Collections.Generic;
using Entities;

namespace DAL.Interfaces
{
    public interface IUserInfoRepository : IRepository<UserInfo>
    {
        void Follow(FollowingSlots item);
        void Unfollow(FollowingSlots item);
        void UnfollowRange(IEnumerable<FollowingSlots> items);
    }
}
