using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Role : IdentityRole<int>
    {
        public Role(string name) : base(name)
        {
            
        }
    }
}
