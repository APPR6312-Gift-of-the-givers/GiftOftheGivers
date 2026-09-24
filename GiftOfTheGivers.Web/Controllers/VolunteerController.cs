using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() => View(new Volunteer());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Volunteer volunteer, string[] skills)
        {
            volunteer.Skills = skills != null ? string.Join(", ", skills) : string.Empty;

            if (!ModelState.IsValid)
            {
                return View(volunteer);
            }

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { name = volunteer.FullName });
        }

        public IActionResult Confirmation(string name)
        {
            ViewData["Name"] = name;
            return View();
        }
    }
}
//Volunteer feature - committed by Avuyile
