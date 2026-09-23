using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models
{
    public class Donation
    {
        public int Id { get; set; }

        [Display(Name = "Donation Type")]
        public string DonationType { get; set; } = "One-Time"; // "One-Time" or "Recurring"

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        [Range(1, 1000000, ErrorMessage = "Please enter a valid amount.")]
        public decimal Amount { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Links the donation to a logged-in donor account, if any (guest donations leave this null).
        public string? DonorUserId { get; set; }

        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime DonatedAt { get; set; } = DateTime.UtcNow;
    }
}
