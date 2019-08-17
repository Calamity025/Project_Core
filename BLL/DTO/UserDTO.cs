using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        public IEnumerable<int> FollowingSlots { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

