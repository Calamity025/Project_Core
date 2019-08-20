using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class FollowingSlots
    {
        public int FollowingUserInfoId { get; set; }
        public UserInfo FollowingUserInfo { get; set; }
        public int SlotId { get; set; }
        public Slot Slot { get; set; }
    }
}
