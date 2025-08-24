using projekatPPP.Models;
using projekatPPP.Repositories;
using projekatPPP.Data;

namespace projekatPPP.Services
{
    public class PredmetService
    {
        private readonly PredmetRepository _repo;
        public PredmetService(PredmetRepository repo) => _repo = repo;

        public IEnumerable<Predmet> GetAllPredmeti() => _repo.GetAll();
        public Predmet? GetPredmet(int id) => _repo.GetById(id);
        public void AddPredmet(Predmet predmet) => _repo.Add(predmet);
        public void UpdatePredmet(Predmet predmet) => _repo.Update(predmet);
        public void DeletePredmet(int id) => _repo.Delete(id);
    }
}