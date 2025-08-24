using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using projekatPPP.Models;

namespace projekatPPP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult MojeOcene()
    {
        // Ovde bi obično išao kod za dobijanje ocena iz baze podataka
        // Za sada, vraćamo praznu listu ocena
        var model = new List<Ocena>();
        return View(model);
    }

    public IActionResult DodajOcenu()
    {
        return View(new Ocena());
    }

    [HttpPost]
    public IActionResult DodajOcenu(Ocena ocena)
    {
        if (ModelState.IsValid)
        {
            // Ovde bi obično išao kod za čuvanje nove ocene u bazi podataka

            return RedirectToAction("MojeOcene");
        }

        return View(ocena);
    }

    public IActionResult IzmeniOcenu(int id)
    {
        // Ovde bi obično išao kod za dobijanje ocene iz baze podataka na osnovu ID-a

        var ocena = new Ocena { Id = id, UcenikId = "1", PredmetId = 1, Vrednost = 5, Komentar = "Odličan napredak!" };
        return View(ocena);
    }

    [HttpPost]
    public IActionResult IzmeniOcenu(Ocena ocena)
    {
        if (ModelState.IsValid)
        {
            // Ovde bi obično išao kod za ažuriranje ocene u bazi podataka

            return RedirectToAction("MojeOcene");
        }

        return View(ocena);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
