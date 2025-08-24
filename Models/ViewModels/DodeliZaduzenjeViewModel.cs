using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace projekatPPP.Models.ViewModels
{
    public class DodeliZaduzenjeViewModel
    {
        // Podaci za formu
        public string NastavnikId { get; set; }
        public int PredmetId { get; set; }
        public int OdeljenjeId { get; set; }

        // Liste za padajuće menije (dropdowns)
        public IEnumerable<SelectListItem> Nastavnici { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Predmeti { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Odeljenja { get; set; } = new List<SelectListItem>();

        // Lista postojećih zaduženja za prikaz
        public IEnumerable<NastavnikPredmet> PostojecaZaduzenja { get; set; } = new List<NastavnikPredmet>();
    }
}