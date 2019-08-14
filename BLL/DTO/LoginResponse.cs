using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BLL.DTO
{
    public class LoginResponse
    {
        public ClaimsIdentity ClaimsIdentity { get; set; }
        public UserDTO User { get; set; }
    }
}
