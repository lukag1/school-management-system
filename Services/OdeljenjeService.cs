using projekatPPP.Models;
using projekatPPP.Repositories;
using System.Collections.Generic;

namespace projekatPPP.Services
{
    public class OdeljenjeService
    {
        private readonly OdeljenjeRepository _repository;

        public OdeljenjeService(OdeljenjeRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Odeljenje> GetAllOdeljenja() => _repository.GetAll();
        public Odeljenje? GetOdeljenje(int id) => _repository.GetById(id);
        public void AddOdeljenje(Odeljenje odeljenje)
        {
            _repository.Add(odeljenje);
            _repository.SaveChanges();
        }
        public void UpdateOdeljenje(Odeljenje odeljenje)
        {
            _repository.Update(odeljenje);
            _repository.SaveChanges();
        }
        public void DeleteOdeljenje(int id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }
    }
}