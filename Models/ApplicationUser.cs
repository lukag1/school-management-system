using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using projekatPPP.Models.Enums; 

namespace projekatPPP.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;

        public int? OdeljenjeId { get; set; }
        public Odeljenje? Odeljenje { get; set; }

        public ICollection<Ocena>? Ocene { get; set; }
        public ICollection<Izostanak>? Izostanci { get; set; } 
    }
}
