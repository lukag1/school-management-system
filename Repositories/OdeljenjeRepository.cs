using projekatPPP.Data;
using projekatPPP.Models;
using System.Collections.Generic;
using System.Linq;

namespace projekatPPP.Repositories
{
    public class OdeljenjeRepository
    {
        private readonly AppDbContext _context;

        public OdeljenjeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Odeljenje> GetAll() => _context.Odeljenja.ToList();
        public Odeljenje? GetById(int id) => _context.Odeljenja.Find(id);
        public void Add(Odeljenje odeljenje) => _context.Odeljenja.Add(odeljenje);
        public void Update(Odeljenje odeljenje) => _context.Odeljenja.Update(odeljenje);
        public void Delete(int id)
        {
            var odeljenje = GetById(id);
            if (odeljenje != null) _context.Odeljenja.Remove(odeljenje);
        }
        public void SaveChanges() => _context.SaveChanges();
    }
}