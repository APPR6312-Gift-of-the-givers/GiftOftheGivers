using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Donate() => View(new Donation());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View(donation);
            }

            donation.CertificateNumber = SeedData.GenerateCertificateNumber();

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                donation.DonorUserId = user?.Id;
            }

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return RedirectToAction("ThankYou", new { id = donation.Id });
        }

        public async Task<IActionResult> ThankYou(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null) return NotFound();
            return View(donation);
        }
    }
}
