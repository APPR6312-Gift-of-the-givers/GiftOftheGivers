using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Web.Models
{
    // Extends the built-in Identity user with a display name.
    // ASP.NET Identity handles password hashing, sign-in, roles, etc. for us.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
