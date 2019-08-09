using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Models
{
    public class CurrentUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarLink { get; set; }
        public bool isAuthorized { get; set; }
    }
}
