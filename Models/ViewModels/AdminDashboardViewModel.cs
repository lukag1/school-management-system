namespace projekatPPP.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; } = new List<UserViewModel>(); // Promenjen tip
        public IEnumerable<Predmet> Predmeti { get; set; } = new List<Predmet>();
        public IEnumerable<Odeljenje> Odeljenja { get; set; } = new List<Odeljenje>();
    }
}