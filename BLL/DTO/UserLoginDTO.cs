using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BLL.DTO
{
    public class UserLoginDTO
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public UserDTO User { get; set; }
        public class UserDTO
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string AvatarLink { get; set; }

        }
    }
}
