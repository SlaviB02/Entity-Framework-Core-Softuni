using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext()
        {
            
        }

        public HospitalContext(DbContextOptions options): base(options) 
        {
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=HospitalDatabase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>()
                .HasKey(e => new { e.PatientId, e.MedicamentId });

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(p => p.Patient)
                .WithMany(pm => pm.Prescriptions);

            modelBuilder.Entity<PatientMedicament>()
                .HasOne(m => m.Medicament)
                .WithMany(pm => pm.Prescriptions);




            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Patient> Patients {  get; set; }   

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament>PatientMedicaments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }
    }
}
