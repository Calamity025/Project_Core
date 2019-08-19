using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Presentation.Models
{
    public class SlotUpdateModel : IValidatableObject
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public ICollection<int> SlotTagIds { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (CategoryId != null && CategoryId <= 0)
            {
                errors.Add(new ValidationResult("Id cannot be 0 or lower", new List<string>(){"CategoryId"}));
            }
            if (SlotTagIds != null && SlotTagIds.Any(x => x <= 0))
            {
                errors.Add(new ValidationResult("Id cannot be 0 or lower", new List<string>() { "SlotTagIds" }));
            }
            return errors;
        }
    }
}
