using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekatPPP.Data;
using projekatPPP.Models;
using projekatPPP.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace projekatPPP.Controllers
{
    [Authorize(Roles = "Ucenik")]
    public class UcenikController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UcenikController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var ucenikId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(ucenikId)) return Unauthorized();

            var ucenik = await _context.Users
                .Include(u => u.Odeljenje)
                .FirstOrDefaultAsync(u => u.Id == ucenikId);

            if (ucenik == null) return NotFound();

            var ocene = await _context.Ocene
                .Include(o => o.Predmet)
                .Where(o => o.UcenikId == ucenikId)
                .ToListAsync();
            
            var izostanci = await _context.Izostanci
                .Where(i => i.UcenikId == ucenikId)
                .ToListAsync();

            var mojiPredmeti = new List<Predmet>();
            if (ucenik.OdeljenjeId.HasValue)
            {
                var predmetIds = await _context.NastavnikPredmeti
                    .Where(np => np.OdeljenjeId == ucenik.OdeljenjeId.Value)
                    .Select(np => np.PredmetId)
                    .Distinct()
                    .ToListAsync();
                
                mojiPredmeti = await _context.Predmeti
                    .Where(p => predmetIds.Contains(p.Id))
                    .OrderBy(p => p.Naziv)
                    .ToListAsync();
            }

            var viewModel = new UcenikDashboardViewModel
            {
                ImePrezime = $"{ucenik.Ime} {ucenik.Prezime}",
                OdeljenjeNaziv = ucenik.Odeljenje?.Naziv ?? "Nije dodeljeno",
                SveOcene = ocene,
                UkupnoIzostanaka = izostanci.Count,
                UkupnoOpravdanih = izostanci.Count(i => i.Opravdan),
                UkupnoNeopravdanih = izostanci.Count(i => !i.Opravdan),
                MojiPredmeti = mojiPredmeti
            };

            if (ocene.Any())
            {
                viewModel.UkupanProsek = ocene.Average(o => o.Vrednost);
                viewModel.ProsekPoPredmetu = ocene
                    .GroupBy(o => o.Predmet?.Naziv ?? "Nepoznat predmet")
                    .ToDictionary(g => g.Key, g => g.Average(o => o.Vrednost));
            }

            return View(viewModel);
        }

        public IActionResult MojeOcene()
        {
            return RedirectToAction("Index");
        }
    }
}