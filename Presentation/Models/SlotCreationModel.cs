using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Presentation.Models
{
    public class SlotCreationModel : IValidatableObject
    {
        [Required]
        [MaxLength(50)]
        public string  Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]
        public decimal? Step { get; set; }
        public int[] SlotTagIds { get; set; }
        [MaxLength(2048)]
        public string Description { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (Price <= 0)
            {
                errors.Add(new ValidationResult("Price cannot be 0 or lower", new List<string>() { "Price" }));
            }
            if (Step <= 0)
            {
                errors.Add(new ValidationResult("Step cannot be 0 or lower", new List<string>() { "Step" }));
            }
            if (SlotTagIds != null && SlotTagIds.Any(x => x <= 0))
            {
                errors.Add(new ValidationResult("Tag id cannot be 0 or lower", new List<string>() { "Tags" }));
            }
            return errors;
        }
    }
}
