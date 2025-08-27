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
        public DbSet<Izostanak> Izostanci { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NastavnikPredmet>()
                .HasKey(np => new { np.NastavnikId, np.PredmetId });

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
