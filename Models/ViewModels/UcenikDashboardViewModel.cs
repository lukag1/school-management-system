using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class UcenikDashboardViewModel
    {
        [Display(Name = "Učenik")]
        public string ImePrezime { get; set; } = string.Empty;

        [Display(Name = "Odeljenje")]
        public string OdeljenjeNaziv { get; set; } = string.Empty;

        public IEnumerable<Ocena> SveOcene { get; set; } = new List<Ocena>();

        [Display(Name = "Ukupan prosek")]
        public double UkupanProsek { get; set; }

        [Display(Name = "Prosek po predmetima")]
        public Dictionary<string, double> ProsekPoPredmetu { get; set; } = new Dictionary<string, double>();

        [Display(Name = "Ukupno izostanaka")]
        public int UkupnoIzostanaka { get; set; }

        [Display(Name = "Opravdani izostanci")]
        public int UkupnoOpravdanih { get; set; }

        [Display(Name = "Neopravdani izostanci")]
        public int UkupnoNeopravdanih { get; set; }

        [Display(Name = "Moji predmeti")]
        public IEnumerable<Predmet> MojiPredmeti { get; set; } = new List<Predmet>();
    }
}