using projekatPPP.Models;
using projekatPPP.Data;

namespace projekatPPP.Repositories
{
    public class OcenaRepository
    {
        private readonly AppDbContext _context;
        public OcenaRepository(AppDbContext context) => _context = context;

        public IEnumerable<Ocena> GetAll() => _context.Ocene.ToList();
        public Ocena? GetById(int id) => _context.Ocene.Find(id);
        public void Add(Ocena ocena) { _context.Ocene.Add(ocena); _context.SaveChanges(); }
        public void Update(Ocena ocena) { _context.Ocene.Update(ocena); _context.SaveChanges(); }
        public void Delete(int id)
        {
            var ocena = _context.Ocene.Find(id);
            if (ocena != null) { _context.Ocene.Remove(ocena); _context.SaveChanges(); }
        }
    }
}