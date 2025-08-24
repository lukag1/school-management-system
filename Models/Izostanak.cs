using System;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models
{
    public class Izostanak
    {
        [Key]
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public bool Opravdan { get; set; }

        // DODATO: Polje za komentar
        public string? Komentar { get; set; }

        public int PredmetId { get; set; }
        public Predmet? Predmet { get; set; }

        public string UcenikId { get; set; } = string.Empty;
        public ApplicationUser? Ucenik { get; set; }
    }
}