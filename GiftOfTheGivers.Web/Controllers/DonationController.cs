using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers
{
    public class DonationController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TaxCertificateService _taxCertificateService;

        public DonationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, TaxCertificateService taxCertificateService)
        {
            _context = context;
            _userManager = userManager;
            _taxCertificateService = taxCertificateService;
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
            var donatioon = new Donation
            {
                DonorUserId = _userManager.GetUserId(User),
                FullName = donation.FullName,
                Amount = donation.Amount,
                DonationType = donation.DonationType,
                Currency = donation.Currency,
            };

            donation.CertificateNumber = await _taxCertificateService.GenerateAsync(donation.FullName,donation.Amount);


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
