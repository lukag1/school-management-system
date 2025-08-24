using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsApproved { get; set; }

        [Display(Name = "Odeljenje")]
        public int? OdeljenjeId { get; set; }

        // DODATO: Polje za ulogu
        [Required(ErrorMessage = "Morate izabrati ulogu.")]
        [Display(Name = "Uloga")]
        public string? Uloga { get; set; }

        public IEnumerable<SelectListItem>? Odeljenja { get; set; }
    }
}