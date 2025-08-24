using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using projekatPPP.Models;

namespace projekatPPP.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Odeljenje> Odeljenja { get; set; }
        public DbSet<Ocena> Ocene { get; set; }
        public DbSet<NastavnikPredmet> NastavnikPredmeti { get; set; }
        public DbSet<Izostanak> Izostanci { get; set; } // Dodaj ovu liniju

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definiši kompozitni ključ za vezu nastavnik-predmet
            modelBuilder.Entity<NastavnikPredmet>()
                .HasKey(np => new { np.NastavnikId, np.PredmetId });

            // Primer konfiguracije za Ocena -> ApplicationUser (string Id)
            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Ucenik)
                .WithMany(u => u.Ocene)
                .HasForeignKey(o => o.UcenikId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ocena>()
                .HasOne(o => o.Nastavnik)
                .WithMany()
                .HasForeignKey(o => o.NastavnikId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
