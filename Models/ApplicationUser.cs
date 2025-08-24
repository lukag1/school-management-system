using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using projekatPPP.Models.Enums; // Dodajte ovu liniju

namespace projekatPPP.Models
{
    public class ApplicationUser : IdentityUser
    {
        // inicijalizovati da se reše CS8618 warnings
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;

        // Veza ka odeljenju
        public int? OdeljenjeId { get; set; }
        public Odeljenje? Odeljenje { get; set; }

        // Navigacije
        public ICollection<Ocena>? Ocene { get; set; }
        public ICollection<Izostanak>? Izostanci { get; set; } // Dodato
    }
}
