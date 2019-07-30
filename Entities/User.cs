using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class User : IdentityUser<int>
    {
        public virtual UserInfo UserInfo { get; set; }
    }
}
