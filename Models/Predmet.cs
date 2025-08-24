using System.Collections.Generic;

namespace projekatPPP.Models
{
    public class Predmet
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty; // Inicijalizacija rešava upozorenje
        public ICollection<NastavnikPredmet> Nastavnici { get; set; } = new List<NastavnikPredmet>();
    }
}
