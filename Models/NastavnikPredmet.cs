namespace projekatPPP.Models
{
    public class NastavnikPredmet
    {
        public string NastavnikId { get; set; } = string.Empty;
        public int PredmetId { get; set; }
        public int OdeljenjeId { get; set; }

        public ApplicationUser? Nastavnik { get; set; }
        public Predmet? Predmet { get; set; }
        public Odeljenje? Odeljenje { get; set; }
    }
}
