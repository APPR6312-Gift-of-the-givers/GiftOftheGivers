using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ContactViewModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        [StringLength(500, MinimumLength = 10)]
        public string Message { get; set; }
    }
}