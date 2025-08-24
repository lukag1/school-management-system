using projekatPPP.Models;
using projekatPPP.Data;

namespace projekatPPP.Repositories
{
    public class PredmetRepository
    {
        private readonly AppDbContext _context;
        public PredmetRepository(AppDbContext context) => _context = context;

        public IEnumerable<Predmet> GetAll() => _context.Predmeti.ToList();
        public Predmet? GetById(int id) => _context.Predmeti.Find(id);
        public void Add(Predmet predmet) { _context.Predmeti.Add(predmet); _context.SaveChanges(); }
        public void Update(Predmet predmet) { _context.Predmeti.Update(predmet); _context.SaveChanges(); }
        public void Delete(int id)
        {
            var predmet = _context.Predmeti.Find(id);
            if (predmet != null) { _context.Predmeti.Remove(predmet); _context.SaveChanges(); }
        }
    }
}