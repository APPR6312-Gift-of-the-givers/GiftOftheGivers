using GiftOfTheGivers.Web.Data;
using GiftOfTheGivers.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Web.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            ViewData["ActiveProjects"] = await _context.ReliefProjects.CountAsync(p => p.IsActive);
            ViewData["UpdatesPosted"] = await _context.FieldUpdates.CountAsync();
            ViewData["VolunteersRegistered"] = await _context.Volunteers.CountAsync();
            ViewData["DonationsThisMonth"] = await _context.Donations
                .Where(d => d.DonatedAt >= startOfMonth)
                .SumAsync(d => (decimal?)d.Amount) ?? 0m;

            ViewData["RecentVolunteers"] = await _context.Volunteers
                .OrderByDescending(v => v.RegisteredAt).Take(5).ToListAsync();
            ViewData["RecentDonations"] = await _context.Donations
                .OrderByDescending(d => d.DonatedAt).Take(5).ToListAsync();
            ViewData["Projects"] = await _context.ReliefProjects.Where(p => p.IsActive).ToListAsync();

            var updates = await _context.FieldUpdates
                .OrderByDescending(u => u.PostedAt).Take(10).ToListAsync();

            return View(updates);
        }

        [HttpGet]
        public async Task<IActionResult> PostUpdate()
        {
            ViewData["Projects"] = await _context.ReliefProjects.Where(p => p.IsActive).ToListAsync();
            return View(new FieldUpdate());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostUpdate(FieldUpdate update)
        {
            var project = await _context.ReliefProjects.FindAsync(update.ReliefProjectId);
            if (project == null)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid project.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Projects"] = await _context.ReliefProjects.Where(p => p.IsActive).ToListAsync();
                return View(update);
            }

            update.ProjectName = project!.Name;
            update.PostedBy = User.Identity?.Name ?? "Employee";
            update.PostedAt = DateTime.UtcNow;

            _context.FieldUpdates.Add(update);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
