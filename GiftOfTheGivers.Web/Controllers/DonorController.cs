using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Controllers
{
    [Authorize(Roles = "Donor")]
    public class DonorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var myDonations = await _context.Donations
                .Where(d => d.DonorUserId == user!.Id)
                .OrderByDescending(d => d.DonatedAt)
                .ToListAsync();

            ViewData["TotalDonated"] = myDonations.Sum(d => d.Amount);
            ViewData["DonationsMade"] = myDonations.Count;
            ViewData["Certificates"] = myDonations.Count;
            ViewData["ActiveProjects"] = await _context.ReliefProjects.Where(p => p.IsActive).ToListAsync();

            return View(myDonations);
        }
    }
}
