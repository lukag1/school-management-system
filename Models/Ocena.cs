using System;

namespace projekatPPP.Models
{
    public class Ocena
    {
        public int Id { get; set; }

        public string UcenikId { get; set; } = string.Empty;
        public string NastavnikId { get; set; } = string.Empty;

        public int PredmetId { get; set; }

        public int Vrednost { get; set; }
        public string Komentar { get; set; } = string.Empty;
        public DateTime Datum { get; set; } = DateTime.UtcNow;

        public ApplicationUser? Ucenik { get; set; }
        public ApplicationUser? Nastavnik { get; set; }
        public Predmet? Predmet { get; set; }
    }
}
