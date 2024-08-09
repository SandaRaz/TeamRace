using Evaluation.Models.Authentication;
using Evaluation.Models.Entity;
using Evaluation.Models.MappingFile;
using Evaluation.Models.MappingView;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Models.Cnx
{
    public class PsqlContext : DbContext
    {
        public PsqlContext(DbContextOptions options) : base(options) { }

        public DbSet<Admin> Admin { get; set;}

        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<Coureur> Coureur { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Etape> Etape { get; set; }
        public DbSet<PointEtape> PointEtape { get; set; }
        public DbSet<Resultat> Resultat { get; set; }
        public DbSet<Penalite> Penalite { get; set; }

        // ------------------------ VIEW ------------------------
        public DbSet<Classement> Classement { get; set; }
        public DbSet<ResultatAvecPenalite> ResultatAvecPenalite { get; set; }
        // ------------------------------------------------------
        // ---------------- MAPPING TABLE TEMPORAIRE ------------
        public DbSet<ClassementGeneraleEquipe> ClassementEquipeCategorie { get; set; }

        // ------------------------------------------------------

        // ------------------------ CSV -------------------------
        public DbSet<CsvEtape> CsvEtape { get; set; }
        public DbSet<CsvResultat> CsvResultat { get; set; }
        public DbSet<CsvPoints> CsvPoints { get; set; }
        // ------------------------------------------------------


        public List<string> ListAllTables()
        {
            List<string> exclude = new List<string>
            {
                "admin",
                "course",
                "categorie"
            };

            var tables = Model.GetEntityTypes()
                              .Select(t => t.GetTableName())
                              .ToList();
            List<string> allTables = new List<string>();
            foreach(var table in tables)
            {
                if(table != null)
                {
                    if (!exclude.Contains(table.ToLower()))
                    {
                        allTables.Add(table);
                    }
                }
            }
            return allTables;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Classement>().HasNoKey().ToView("Classement");
            modelBuilder.Entity<ResultatAvecPenalite>().HasNoKey().ToView("ResultatAvecPenalite");
            modelBuilder.Entity<ClassementGeneraleEquipe>().HasNoKey();

            modelBuilder.Entity<Coureur>()
            .HasIndex(c => c.NumDossard)
            .IsUnique();

            modelBuilder.Entity<Coureur>()
                .HasMany(c => c.Categories)
                .WithMany(c => c.Coureurs)
                .UsingEntity<Dictionary<string, object>>(
                    "CoureurCategorie",
                    j => j.HasOne<Categorie>().WithMany().HasForeignKey("CategorieId"),
                    j => j.HasOne<Coureur>().WithMany().HasForeignKey("CoureurId")
                );

            modelBuilder.Entity<Coureur>()
                .HasMany(c => c.Resultats)
                .WithOne(r => r.Coureur)
                .HasForeignKey(r => r.CoureurId);

            modelBuilder.Entity<Equipe>()
                .HasMany(e => e.Coureurs)
                .WithOne(c => c.Equipe)
                .HasForeignKey(c => c.EquipeId);

            modelBuilder.Entity<Etape>()
                .HasMany(e => e.Coureurs)
                .WithMany(c => c.Etapes)
                .UsingEntity<Dictionary<string, object>>(
                    "CoureurEtape",
                    j => j.HasOne<Coureur>().WithMany().HasForeignKey("CoureurId"),
                    j => j.HasOne<Etape>().WithMany().HasForeignKey("EtapeId")
                );

            modelBuilder.Entity<Etape>()
                .HasMany(e => e.Resultats)
                .WithOne(r => r.Etape)
                .HasForeignKey(r => r.EtapeId);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Etapes)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId);

            

        }
    }  
}
