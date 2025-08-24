using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class DodajOcenuViewModel
    {
        public string UcenikId { get; set; }
        
        // Promena je ovde: string? označava da polje može biti null
        public string? UcenikImePrezime { get; set; }

        [Required(ErrorMessage = "Morate izabrati predmet.")]
        [Display(Name = "Predmet")]
        public int PredmetId { get; set; }

        [Required(ErrorMessage = "Ocena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena")]
        public int Vrednost { get; set; }

        [Display(Name = "Komentar / Povratna informacija")]
        public string? Komentar { get; set; }

        public IEnumerable<SelectListItem> Predmeti { get; set; } = new List<SelectListItem>();
    }
}