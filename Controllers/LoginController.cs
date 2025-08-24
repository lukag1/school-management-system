using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekatPPP.Models;
using System.Linq;
using System.Threading.Tasks;

namespace projekatPPP.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string lozinka)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(lozinka))
            {
                ViewBag.Error = "Email i lozinka su obavezni.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Neispravni kredencijali.";
                return View();
            }

            if (!user.IsApproved)
            {
                ViewBag.Error = "Vaš nalog još uvek nije odobren od strane administratora.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, lozinka, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                // Ispravna logika za preusmeravanje
                return role switch
                {
                    "Administrator" => RedirectToAction("Index", "Admin"),
                    "Nastavnik" => RedirectToAction("Index", "Nastavnik"),
                    "Ucenik" => RedirectToAction("Index", "Ucenik"),
                    _ => RedirectToAction("Index", "/") // Podrazumevana stranica
                };
            }

            ViewBag.Error = "Neispravni kredencijali.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "/");
        }
    }
}