using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class EditOcenaViewModel
    {
        public int OcenaId { get; set; }
        public string? UcenikImePrezime { get; set; }
        public string? PredmetNaziv { get; set; }

        [Required(ErrorMessage = "Ocena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocena mora biti između 1 i 5.")]
        [Display(Name = "Ocena")]
        public int Vrednost { get; set; }

        [Display(Name = "Komentar / Povratna informacija")]
        public string? Komentar { get; set; }
    }
}