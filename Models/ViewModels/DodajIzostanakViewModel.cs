using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class DodajIzostanakViewModel
    {
        public string UcenikId { get; set; } = string.Empty;
        public string? UcenikImePrezime { get; set; }

        [Required(ErrorMessage = "Morate izabrati predmet.")]
        [Display(Name = "Predmet sa kog je učenik odsustvovao")]
        public int PredmetId { get; set; }

        [Display(Name = "Da li je izostanak opravdan?")]
        public bool Opravdan { get; set; }

        [Display(Name = "Komentar (npr. razlog izostanka)")]
        public string? Komentar { get; set; }

        public IEnumerable<SelectListItem> Predmeti { get; set; } = new List<SelectListItem>();
    }
}