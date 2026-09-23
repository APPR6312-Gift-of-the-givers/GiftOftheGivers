using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Stored as a comma-separated list of the skill tags the volunteer picked.
        [Display(Name = "Skills & Expertise")]
        public string Skills { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Availability")]
        public string Availability { get; set; } = string.Empty;

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
