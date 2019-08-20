using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class UserInfo
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DefaultValue(0)]
        public decimal Balance { get; set; }
        public string ImageLink { get; set; }
        public virtual ICollection<FollowingSlots> FollowingSlots { get; }
        public virtual ICollection<BetHistory> BetSlots { get; }
        public virtual ICollection<Slot> PlacedSlots { get; set; }
        public virtual ICollection<Slot> WonSlots { get; set; }

        public UserInfo()
        {
            FollowingSlots = new List<FollowingSlots>();
            BetSlots = new List<BetHistory>();
            PlacedSlots = new List<Slot>();
            WonSlots = new List<Slot>();
        }
    }
}
