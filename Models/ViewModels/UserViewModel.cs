namespace projekatPPP.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Uloga { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
    }
}