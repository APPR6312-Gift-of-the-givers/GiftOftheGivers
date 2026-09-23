using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Data
{
    // IdentityDbContext gives us Users, Roles, Claims, Logins etc. for free.
    // We add our own DbSets for the domain data the POE asks for.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<Donation> Donations => Set<Donation>();
        public DbSet<ReliefProject> ReliefProjects => Set<ReliefProject>();
        public DbSet<FieldUpdate> FieldUpdates => Set<FieldUpdate>();
    }
}
