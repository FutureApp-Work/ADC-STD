using dotnet.models.testing.Entities;
using Microsoft.EntityFrameworkCore;

namespace dotnet.models.testing.Data
{
    public class AdcDbContext : DbContext
    {
        public AdcDbContext(DbContextOptions<AdcDbContext> options) : base(options)
        {
        }

        // Main entities
        public DbSet<User> Users { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<MedicationKnowledge> MedicationKnowledges { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Account).IsUnique();
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            // Cabinet configuration
            modelBuilder.Entity<Cabinet>(entity =>
            {
                entity.ToTable("Cabinet");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            // MedicationKnowledge configuration
            modelBuilder.Entity<MedicationKnowledge>(entity =>
            {
                entity.ToTable("MedicationKnowledge");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
                entity.Property(e => e.CommonAmount).HasPrecision(18, 2);
            });

            // Patient configuration
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patient");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Number);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
                entity.HasOne(e => e.Station)
                      .WithMany(s => s.Patients)
                      .HasForeignKey(e => e.StationId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Station configuration
            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("Station");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            // Prescription configuration
            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.ToTable("Prescription");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Number);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
                entity.Property(e => e.OpeningTime).HasColumnType("datetime");
                entity.Property(e => e.DeliveryDate).HasColumnType("date");
                entity.Property(e => e.DeleteDate).HasColumnType("date");
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Prescriptions)
                      .HasForeignKey(e => e.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // PrescriptionDetail configuration
            modelBuilder.Entity<PrescriptionDetail>(entity =>
            {
                entity.ToTable("PrescriptionDetail");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.DetailNumber);
                entity.Property(e => e.CreateTime).HasColumnType("datetime");
                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
                entity.Property(e => e.DemandAmount).HasPrecision(18, 2);
                entity.Property(e => e.LastTimeToGet).HasColumnType("datetime");
                entity.Property(e => e.ApprovalTime).HasColumnType("datetime");
                entity.HasOne(e => e.Prescription)
                      .WithMany(p => p.PrescriptionDetails)
                      .HasForeignKey(e => e.PrescriptionId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.MedicationKnowledge)
                      .WithMany(m => m.PrescriptionDetails)
                      .HasForeignKey(e => e.MedicationKnowledgeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
