
namespace Entities
{
    public class BetHistory
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public Slot Slot { get; set; }
        public int BetUserInfoId { get; set; }
        public UserInfo BetUserInfo { get; set; }
        public decimal Price { get; set; }
    }
}
