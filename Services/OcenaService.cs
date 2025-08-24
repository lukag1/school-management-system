using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using projekatPPP.Models;
using projekatPPP.Repositories;

namespace projekatPPP.Services
{
    public class OcenaService
    {
        private readonly OcenaRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OcenaService(OcenaRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Ocena> GetAllOcene() => _repo.GetAll();
        public Ocena? GetOcena(int id) => _repo.GetById(id);

        public void AddOcena(Ocena ocena)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.IsInRole("Nastavnik"))
                throw new UnauthorizedAccessException("Samo nastavnik može unositi ocene.");

            var nastavnikId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nastavnikId))
                throw new UnauthorizedAccessException("Nevažeći nastavnik.");

            // Ako nije postavljen, dodeli nastavnikId iz prilikom unosa
            if (string.IsNullOrEmpty(ocena.NastavnikId))
                ocena.NastavnikId = nastavnikId;

            if (nastavnikId != ocena.NastavnikId)
                throw new InvalidOperationException("Nastavnik može unositi samo svoje ocene.");

            _repo.Add(ocena);
        }

        public void UpdateOcena(Ocena ocena)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new UnauthorizedAccessException("Neautorizovan pristup.");

            var existing = _repo.GetById(ocena.Id);
            if (existing == null)
                throw new InvalidOperationException("Ocena ne postoji.");

            var nastavnikId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = user.IsInRole("Administrator");

            // nastavnik može ažurirati samo svoje ocene, admin može sve
            if (!isAdmin)
            {
                if (string.IsNullOrEmpty(nastavnikId) || nastavnikId != existing.NastavnikId)
                    throw new UnauthorizedAccessException("Nastavnik može menjati samo svoje ocene.");
            }

            _repo.Update(ocena);
        }

        public void DeleteOcena(int id)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
                throw new UnauthorizedAccessException("Neautorizovan pristup.");

            var existing = _repo.GetById(id);
            if (existing == null)
                return;

            var nastavnikId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = user.IsInRole("Administrator");

            if (!isAdmin)
            {
                if (string.IsNullOrEmpty(nastavnikId) || nastavnikId != existing.NastavnikId)
                    throw new UnauthorizedAccessException("Nastavnik može brisati samo svoje ocene.");
            }

            _repo.Delete(id);
        }
    }
}