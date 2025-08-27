using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Ime { get; set; } = string.Empty;

        [Required]
        public string Prezime { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool IsApproved { get; set; }

        [Display(Name = "Odeljenje")]
        public int? OdeljenjeId { get; set; }

        [Required(ErrorMessage = "Morate izabrati ulogu.")]
        [Display(Name = "Uloga")]
        public string? Uloga { get; set; }

        public IEnumerable<SelectListItem>? Odeljenja { get; set; }
    }
}