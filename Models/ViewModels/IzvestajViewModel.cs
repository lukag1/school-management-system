using System.Collections.Generic;

namespace projekatPPP.Models.ViewModels
{
    public class IzvestajViewModel
    {
        public int UkupnoKorisnika { get; set; }
        public int UkupnoNastavnika { get; set; }
        public int UkupnoUcenika { get; set; }
        public int UkupnoPredmeta { get; set; }
        public int UkupnoOdeljenja { get; set; }
        public double ProsekSvihOcena { get; set; }
        public Dictionary<string, double> ProsekPoPredmetima { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, int> BrojUcenikaPoOdeljenju { get; set; } = new Dictionary<string, int>();
    }
}