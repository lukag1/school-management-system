using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekatPPP.Data;
using projekatPPP.Models;
using projekatPPP.Models.ViewModels;
using projekatPPP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projekatPPP.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly PredmetService _predmetService;
        private readonly OdeljenjeService _odeljenjeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserService userService, PredmetService predmetService, OdeljenjeService odeljenjeService, UserManager<ApplicationUser> userManager, AppDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userService = userService;
            _predmetService = predmetService;
            _odeljenjeService = odeljenjeService;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userService.GetAllUsers();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Ime = user.Ime,
                    Prezime = user.Prezime,
                    Email = user.Email,
                    IsApproved = user.IsApproved,
                    Uloga = roles.FirstOrDefault() ?? "Nema ulogu"
                });
            }

            var viewModel = new AdminDashboardViewModel
            {
                Users = userViewModels,
                Predmeti = _predmetService.GetAllPredmeti(),
                Odeljenja = _odeljenjeService.GetAllOdeljenja()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Users()
        {
            var users = _userService.GetAllUsers();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Ime = user.Ime,
                    Prezime = user.Prezime,
                    Email = user.Email,
                    IsApproved = user.IsApproved,
                    Uloga = roles.FirstOrDefault() ?? "Nema ulogu"
                });
            }
            return View(userViewModels);
        }

        public IActionResult CreateUser() => View(new ApplicationUser());

        [HttpPost]
        public async Task<IActionResult> CreateUser(ApplicationUser user, string lozinka, string uloga)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                ModelState.AddModelError("Email", "Email je obavezno polje.");
                return View(user);
            }

            var postojeciKorisnik = await _userManager.FindByEmailAsync(user.Email);
            if (postojeciKorisnik != null)
            {
                ModelState.AddModelError("Email", "Korisnik sa ovim emailom već postoji.");
                ViewBag.Error = $"Korisnik sa emailom {user.Email} već postoji. ";
                ViewBag.ExistingUserId = postojeciKorisnik.Id;
                return View(user);
            }

            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, lozinka);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, uloga);

                if (uloga == "Ucenik")
                {
                    var odeljenjeZaDodavanje = await _context.Odeljenja
                        .Where(o => _context.Users.Count(u => u.OdeljenjeId == o.Id) < 20)
                        .OrderBy(o => _context.Users.Count(u => u.OdeljenjeId == o.Id))
                        .FirstOrDefaultAsync();

                    if (odeljenjeZaDodavanje != null)
                    {
                        user.OdeljenjeId = odeljenjeZaDodavanje.Id;
                        await _userManager.UpdateAsync(user);
                    }
                }
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(user);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userService.GetUser(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Ime = user.Ime,
                Prezime = user.Prezime,
                Email = user.Email,
                IsApproved = user.IsApproved,
                OdeljenjeId = user.OdeljenjeId,
                Uloga = roles.FirstOrDefault(),
                Odeljenja = _context.Odeljenja.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = o.Naziv
                })
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var postojeciKorisnik = await _userManager.FindByIdAsync(model.Id);
            if (postojeciKorisnik == null)
            {
                return NotFound();
            }

            var stareUloge = await _userManager.GetRolesAsync(postojeciKorisnik);
            string staraUloga = stareUloge.FirstOrDefault() ?? "";

            postojeciKorisnik.Ime = model.Ime;
            postojeciKorisnik.Prezime = model.Prezime;
            postojeciKorisnik.OdeljenjeId = model.OdeljenjeId;
            postojeciKorisnik.Email = model.Email;
            postojeciKorisnik.UserName = model.Email;

            await _userManager.UpdateAsync(postojeciKorisnik);

            if (staraUloga != model.Uloga)
            {
                if (!string.IsNullOrEmpty(staraUloga))
                {
                    await _userManager.RemoveFromRoleAsync(postojeciKorisnik, staraUloga);
                }
                if (!string.IsNullOrEmpty(model.Uloga))
                {
                    await _userManager.AddToRoleAsync(postojeciKorisnik, model.Uloga);
                }

                if (staraUloga == "Ucenik")
                {
                    var ocene = _context.Ocene.Where(o => o.UcenikId == model.Id);
                    _context.Ocene.RemoveRange(ocene);
                    var izostanci = _context.Izostanci.Where(i => i.UcenikId == model.Id);
                    _context.Izostanci.RemoveRange(izostanci);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Korisnik ažuriran. Obrisane su njegove ocene i izostanci zbog promene uloge.";
                }
                else if (staraUloga == "Nastavnik")
                {
                    var zaduzenja = _context.NastavnikPredmeti.Where(np => np.NastavnikId == model.Id);
                    _context.NastavnikPredmeti.RemoveRange(zaduzenja);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Korisnik ažuriran. Obrisana su njegova zaduženja zbog promene uloge.";
                }
                else
                {
                    TempData["Message"] = "Korisnik uspešno ažuriran.";
                }
            }
            else
            {
                TempData["Message"] = "Korisnik uspešno ažuriran.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost] 
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveUser(string id)
        {
            await _userService.ApproveUser(id);
            return RedirectToAction("Index");
        }

        public IActionResult Predmeti() => View(_predmetService.GetAllPredmeti());
        public IActionResult CreatePredmet() => View(new Predmet());
        [HttpPost]
        public IActionResult CreatePredmet(Predmet predmet)
        {
            _predmetService.AddPredmet(predmet);
            return RedirectToAction("Index");
        }
        public IActionResult EditPredmet(int id) => View(_predmetService.GetPredmet(id));
        [HttpPost]
        public IActionResult EditPredmet(Predmet predmet)
        {
            _predmetService.UpdatePredmet(predmet);
            return RedirectToAction("Index");
        }

        [HttpPost] 
        public async Task<IActionResult> DeletePredmet(int id)
        {
            var predmet = await _context.Predmeti.FindAsync(id);
            if (predmet == null)
            {
                return NotFound();
            }

            bool imaOcena = await _context.Ocene.AnyAsync(o => o.PredmetId == id);
            bool imaZaduzenja = await _context.NastavnikPredmeti.AnyAsync(np => np.PredmetId == id);
            bool imaIzostanaka = await _context.Izostanci.AnyAsync(i => i.PredmetId == id);

            if (imaOcena || imaZaduzenja || imaIzostanaka)
            {
                TempData["Error"] = "Nije moguće obrisati predmet jer postoje povezani podaci (ocene, zaduženja ili izostanci).";
                return RedirectToAction("Index");
            }

            _context.Predmeti.Remove(predmet);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Predmet je uspešno obrisan.";
            return RedirectToAction("Index");
        }

        public IActionResult CreateOdeljenje() => View(new Odeljenje());
        [HttpPost]
        public IActionResult CreateOdeljenje(Odeljenje odeljenje)
        {
            _odeljenjeService.AddOdeljenje(odeljenje);
            return RedirectToAction("Index");
        }
        public IActionResult EditOdeljenje(int id) => View(_odeljenjeService.GetOdeljenje(id));
        [HttpPost]
        public IActionResult EditOdeljenje(Odeljenje odeljenje)
        {
            _odeljenjeService.UpdateOdeljenje(odeljenje);
            return RedirectToAction("Index");
        }

        [HttpPost] 
        public async Task<IActionResult> DeleteOdeljenje(int id)
        {
            var odeljenje = await _context.Odeljenja.FindAsync(id);
            if (odeljenje == null)
            {
                return NotFound();
            }

            bool imaUcenika = await _context.Users.AnyAsync(u => u.OdeljenjeId == id);
            bool imaZaduzenja = await _context.NastavnikPredmeti.AnyAsync(np => np.OdeljenjeId == id);

            if (imaUcenika || imaZaduzenja)
            {
                TempData["Error"] = "Nije moguće obrisati odeljenje jer postoje povezani podaci (učenici ili zaduženja).";
                return RedirectToAction("Index");
            }

            _context.Odeljenja.Remove(odeljenje);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Odeljenje je uspešno obrisano.";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> DodeliZaduzenje()
        {
            var nastavnici = await _userManager.GetUsersInRoleAsync("Nastavnik");

            var viewModel = new DodeliZaduzenjeViewModel
            {
                Nastavnici = nastavnici.Select(n => new SelectListItem { Value = n.Id, Text = $"{n.Ime} {n.Prezime}" }),
                Predmeti = _context.Predmeti.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Naziv }),
                Odeljenja = _context.Odeljenja.Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.Naziv }),
                PostojecaZaduzenja = await _context.NastavnikPredmeti
                                            .Include(z => z.Nastavnik)
                                            .Include(z => z.Predmet)
                                            .Include(z => z.Odeljenje)
                                            .ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DodeliZaduzenje(DodeliZaduzenjeViewModel model)
        {
            var zaduzenje = new NastavnikPredmet
            {
                NastavnikId = model.NastavnikId,
                PredmetId = model.PredmetId,
                OdeljenjeId = model.OdeljenjeId
            };

            var postoji = await _context.NastavnikPredmeti.AnyAsync(z => z.NastavnikId == model.NastavnikId && z.PredmetId == model.PredmetId && z.OdeljenjeId == model.OdeljenjeId);
            if (!postoji)
            {
                _context.NastavnikPredmeti.Add(zaduzenje);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("DodeliZaduzenje");
        }

        [HttpPost]
        public async Task<IActionResult> UkloniZaduzenje(string nastavnikId, int predmetId, int odeljenjeId)
        {
            var zaduzenje = await _context.NastavnikPredmeti.FindAsync(nastavnikId, predmetId, odeljenjeId);
            if (zaduzenje != null)
            {
                _context.NastavnikPredmeti.Remove(zaduzenje);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("DodeliZaduzenje");
        }

        public async Task<IActionResult> Izvestaji()
        {
            var ucenici = await _context.Users
                .Include(u => u.Odeljenje)
                .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Ucenik")))
                .ToListAsync();

            var ocene = await _context.Ocene.Include(o => o.Predmet).ToListAsync();
            var nastavnici = await _userManager.GetUsersInRoleAsync("Nastavnik");

            var viewModel = new IzvestajViewModel
            {
                UkupnoKorisnika = await _context.Users.CountAsync(),
                UkupnoNastavnika = nastavnici.Count,
                UkupnoUcenika = ucenici.Count,
                UkupnoPredmeta = await _context.Predmeti.CountAsync(),
                UkupnoOdeljenja = await _context.Odeljenja.CountAsync(),
                ProsekSvihOcena = ocene.Any() ? ocene.Average(o => o.Vrednost) : 0,
                ProsekPoPredmetima = ocene
                                          .Where(o => o.Predmet != null)
                                          .GroupBy(o => o.Predmet!.Naziv)
                                          .ToDictionary(g => g.Key, g => g.Average(o => o.Vrednost)),
                BrojUcenikaPoOdeljenju = ucenici
                                                .Where(u => u.Odeljenje != null)
                                                .GroupBy(u => u.Odeljenje!.Naziv)
                                                .ToDictionary(g => g.Key, g => g.Count())
            };
            return View(viewModel);
        }
    }
}