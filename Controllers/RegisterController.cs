using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekatPPP.Models;
using projekatPPP.Models.ViewModels;
using System.Threading.Tasks;

namespace projekatPPP.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    IsApproved = false
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Ucenik");
                    return RedirectToAction("RegistrationSuccess");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }
    }
}