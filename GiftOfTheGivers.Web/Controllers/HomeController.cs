using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        [Route("Home/Error")]
        public IActionResult Error() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Process the message (send email, save to database)
                // For prototype, just show success message
                TempData["SuccessMessage"] = "Thank you for your message. We'll get back to you soon!";
                return RedirectToAction("Contact");
            }
            return View(model);
        }
    }
}


