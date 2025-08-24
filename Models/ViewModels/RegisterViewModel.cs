using System.ComponentModel.DataAnnotations;

namespace projekatPPP.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ime je obavezno.")]
        [Display(Name = "Ime")]
        public string Ime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [StringLength(100, ErrorMessage = "{0} mora imati najmanje {2} karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi lozinku")]
        [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke se ne poklapaju.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}