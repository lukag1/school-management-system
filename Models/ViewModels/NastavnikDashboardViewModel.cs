using System.Collections.Generic;

namespace projekatPPP.Models.ViewModels
{
    public class NastavnikDashboardViewModel
    {
        public string NastavnikImePrezime { get; set; } = string.Empty;

        public IEnumerable<Predmet> MojiPredmeti { get; set; } = new List<Predmet>();
        public IEnumerable<Odeljenje> MojaOdeljenja { get; set; } = new List<Odeljenje>();
        public IEnumerable<ApplicationUser> Ucenici { get; set; } = new List<ApplicationUser>();

        public IEnumerable<Ocena> NedavneOcene { get; set; } = new List<Ocena>();

        public Dictionary<string, double> ProsekPoPredmetima { get; set; } = new Dictionary<string, double>();
    }
}