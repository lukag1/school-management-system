using projekatPPP.Models;
using projekatPPP.Data;

namespace projekatPPP.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public IEnumerable<ApplicationUser> GetAll() => _context.Users.ToList();
        public ApplicationUser? GetById(int id) => _context.Users.Find(id);
        public void Add(ApplicationUser user) { _context.Users.Add(user); _context.SaveChanges(); }
        public void Update(ApplicationUser user) { _context.Users.Update(user); _context.SaveChanges(); }
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null) { _context.Users.Remove(user); _context.SaveChanges(); }
        }
    }
}