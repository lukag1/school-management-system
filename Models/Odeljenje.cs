using System.Collections.Generic;

namespace projekatPPP.Models
{
    public class Odeljenje
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty; 
        public int Razred { get; set; } 
        public ICollection<ApplicationUser> Ucenici { get; set; } = new List<ApplicationUser>();
    }
}
