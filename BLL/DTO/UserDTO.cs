using System.Collections.Generic;

namespace BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        public IEnumerable<int> FollowingSlots { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public decimal Balance { get; set; }
    }
}

