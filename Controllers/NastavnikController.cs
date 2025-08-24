using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekatPPP.Data;
using projekatPPP.Models;
using projekatPPP.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace projekatPPP.Controllers
{
    [Authorize(Roles = "Nastavnik")]
    public class NastavnikController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NastavnikController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nastavnik = await _userManager.FindByIdAsync(nastavnikId);
            if (nastavnik == null) return Unauthorized();

            var assignments = await _context.NastavnikPredmeti
                .Include(np => np.Predmet)
                .Include(np => np.Odeljenje)
                .Where(np => np.NastavnikId == nastavnikId)
                .ToListAsync();

            var odeljenjeIds = assignments.Select(a => a.OdeljenjeId).Distinct();
            
            var ucenici = await _context.Users
                .Include(u => u.Odeljenje)
                .Where(u => u.OdeljenjeId.HasValue && odeljenjeIds.Contains(u.OdeljenjeId.Value))
                .OrderBy(u => u.Prezime)
                .ToListAsync();

            var ocene = await _context.Ocene
                .Include(o => o.Predmet)
                .Include(o => o.Ucenik)
                .Where(o => o.NastavnikId == nastavnikId)
                .ToListAsync();

            var viewModel = new NastavnikDashboardViewModel
            {
                NastavnikImePrezime = $"{nastavnik.Ime} {nastavnik.Prezime}",
                MojiPredmeti = assignments.Select(a => a.Predmet).Where(p => p != null).Distinct(),
                MojaOdeljenja = assignments.Select(a => a.Odeljenje).Where(o => o != null).Distinct(),
                Ucenici = ucenici,
                NedavneOcene = ocene.OrderByDescending(o => o.Datum).Take(5),
                ProsekPoPredmetima = ocene.Any() ? ocene
                    .GroupBy(o => o.Predmet.Naziv)
                    .ToDictionary(g => g.Key, g => g.Average(o => o.Vrednost)) : new Dictionary<string, double>()
            };

            return View(viewModel);
        }

        // GET: /Nastavnik/DodajOcenu?ucenikId=...
        [HttpGet]
        public async Task<IActionResult> DodajOcenu(string ucenikId)
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Dodato
            var ucenik = await _context.Users.FindAsync(ucenikId);

            if (ucenik == null || ucenik.OdeljenjeId == null) return NotFound();

            var predmeti = await _context.NastavnikPredmeti
                .Where(np => np.NastavnikId == nastavnikId && np.OdeljenjeId == ucenik.OdeljenjeId)
                .Select(np => np.Predmet)
                .ToListAsync();

            var viewModel = new DodajOcenuViewModel
            {
                UcenikId = ucenik.Id,
                UcenikImePrezime = $"{ucenik.Ime} {ucenik.Prezime}",
                Predmeti = new SelectList(predmeti, "Id", "Naziv")
            };

            return View(viewModel);
        }

        // POST: /Nastavnik/DodajOcenu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajOcenu(DodajOcenuViewModel model)
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                var ocena = new Ocena
                {
                    Vrednost = model.Vrednost,
                    Komentar = model.Komentar,
                    Datum = DateTime.Now,
                    PredmetId = model.PredmetId,
                    UcenikId = model.UcenikId,
                    NastavnikId = nastavnikId
                };

                _context.Ocene.Add(ocena);
                await _context.SaveChangesAsync();
                
                // Dodaj success poruku u TempData
                TempData["Success"] = "Ocena je uspešno dodata!";
                
                return RedirectToAction("Index");
            }
            
            // Ako model nije validan, ponovo pripremi podatke za formu i vrati isti view
            var ucenik = await _context.Users.FindAsync(model.UcenikId);
            if (ucenik != null)
            {
                model.UcenikImePrezime = $"{ucenik.Ime} {ucenik.Prezime}";
                if (ucenik.OdeljenjeId.HasValue)
                {
                    var predmeti = await _context.NastavnikPredmeti
                        .Where(np => np.NastavnikId == nastavnikId && np.OdeljenjeId == ucenik.OdeljenjeId)
                        .Select(np => np.Predmet).ToListAsync();
                    model.Predmeti = new SelectList(predmeti, "Id", "Naziv", model.PredmetId);
                }
            }
       
            return View(model);
        }

        // GET: /Nastavnik/PregledOcena?ucenikId=...
        [HttpGet]
        public async Task<IActionResult> PregledOcena(string ucenikId)
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ucenik = await _context.Users.FindAsync(ucenikId);
            if (ucenik == null) return NotFound();

            var ocene = await _context.Ocene
                .Include(o => o.Predmet)
                .Where(o => o.UcenikId == ucenikId && o.NastavnikId == nastavnikId)
                .OrderByDescending(o => o.Datum)
                .ToListAsync();

            var viewModel = new PregledOcenaViewModel
            {
                UcenikImePrezime = $"{ucenik.Ime} {ucenik.Prezime}",
                Ocene = ocene
            };

            return View(viewModel);
        }

        // GET: /Nastavnik/EditOcena/{ocenaId}
        [HttpGet]
        public async Task<IActionResult> EditOcena(int ocenaId)
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ocena = await _context.Ocene
                .Include(o => o.Ucenik)
                .Include(o => o.Predmet)
                .FirstOrDefaultAsync(o => o.Id == ocenaId);

            // Provera da li ocena postoji i da li pripada prijavljenom nastavniku
            if (ocena == null || ocena.NastavnikId != nastavnikId)
            {
                return NotFound();
            }

            var viewModel = new EditOcenaViewModel
            {
                OcenaId = ocena.Id,
                UcenikImePrezime = $"{ocena.Ucenik?.Ime} {ocena.Ucenik?.Prezime}",
                PredmetNaziv = ocena.Predmet?.Naziv,
                Vrednost = ocena.Vrednost,
                Komentar = ocena.Komentar
            };

            return View(viewModel);
        }

        // POST: /Nastavnik/EditOcena
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOcena(EditOcenaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var ocena = await _context.Ocene.FindAsync(model.OcenaId);

                if (ocena == null || ocena.NastavnikId != nastavnikId)
                {
                    return NotFound();
                }

                ocena.Vrednost = model.Vrednost;
                ocena.Komentar = model.Komentar;
                ocena.Datum = DateTime.Now; // Ažuriraj datum na datum izmene

                _context.Update(ocena);
                await _context.SaveChangesAsync();

                return RedirectToAction("PregledOcena", new { ucenikId = ocena.UcenikId });
            }
            return View(model);
        }

        // GET: /Nastavnik/DodajIzostanak?ucenikId=...
        [HttpGet]
        public async Task<IActionResult> DodajIzostanak(string ucenikId)
        {
            var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ucenik = await _context.Users.FindAsync(ucenikId);

            if (ucenik == null || ucenik.OdeljenjeId == null) return NotFound();

            var predmeti = await _context.NastavnikPredmeti
                .Where(np => np.NastavnikId == nastavnikId && np.OdeljenjeId == ucenik.OdeljenjeId)
                .Select(np => np.Predmet)
                .ToListAsync();

            var viewModel = new DodajIzostanakViewModel
            {
                UcenikId = ucenik.Id,
                UcenikImePrezime = $"{ucenik.Ime} {ucenik.Prezime}",
                Predmeti = new SelectList(predmeti, "Id", "Naziv")
            };

            return View(viewModel);
        }

        // POST: /Nastavnik/DodajIzostanak
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajIzostanak(DodajIzostanakViewModel model)
        {
            if (ModelState.IsValid)
            {
                var izostanak = new Izostanak
                {
                    Datum = DateTime.Now,
                    Opravdan = model.Opravdan,
                    Komentar = model.Komentar,
                    PredmetId = model.PredmetId,
                    UcenikId = model.UcenikId
                };

                _context.Izostanci.Add(izostanak);
                await _context.SaveChangesAsync();
                
                // Dodaj success poruku u TempData
                TempData["Success"] = "Izostanak je uspešno evidentiran!";
                
                return RedirectToAction("Index");
            }

            // Ako model nije validan, ponovo popuni podatke za formu
            var ucenik = await _context.Users.FindAsync(model.UcenikId);
            if (ucenik != null)
            {
                model.UcenikImePrezime = $"{ucenik.Ime} {ucenik.Prezime}";
                var nastavnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var predmeti = await _context.NastavnikPredmeti
                    .Where(np => np.NastavnikId == nastavnikId && np.OdeljenjeId == ucenik.OdeljenjeId)
                    .Select(np => np.Predmet).ToListAsync();
                model.Predmeti = new SelectList(predmeti, "Id", "Naziv", model.PredmetId);
            }

            return View(model);
        }
    }
}