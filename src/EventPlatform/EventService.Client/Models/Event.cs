using System.ComponentModel.DataAnnotations;

namespace EventService.Client.Models
{
    public class Event : IValidatableObject
    {
        public Event()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public Guid Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult(
                    "Start date must be before end date",
                    new[] { nameof(StartDate) });
            }
        }
    }
}
