using Microsoft.AspNetCore.Identity;

namespace Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role(string name) : base(name)
        {
            
        }
    }
}
