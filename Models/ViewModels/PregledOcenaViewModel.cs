using System.Collections.Generic;

namespace projekatPPP.Models.ViewModels
{
    public class PregledOcenaViewModel
    {
        public string UcenikImePrezime { get; set; } = string.Empty;
        public List<Ocena> Ocene { get; set; } = new List<Ocena>();
    }
}