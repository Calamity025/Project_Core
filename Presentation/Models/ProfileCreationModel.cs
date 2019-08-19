using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class ProfileCreationModel
    {
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
    }
}
