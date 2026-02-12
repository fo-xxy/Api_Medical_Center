using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Entidades para crealas
        public DbSet<Users> Users { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<ClaimImports> ClaimImports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Parametrización para la entidad Users
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.HasIndex(e => e.email).IsUnique(); 
            });

            //Parametrización para la entidad Patients
            modelBuilder.Entity<Patients>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            //Parametrización para la entidad ClaimImports
            modelBuilder.Entity<ClaimImports>(entity =>
            {
                entity.HasKey(e => e.id);
            });

            //Parametrización para la entindad Claims
            modelBuilder.Entity<Claims>(entity =>
            {
                entity.HasKey(e => e.id);

                //Relación con Pacientes
                entity.HasOne(e => e.Patient)
                      .WithMany(p => p.Claims)
                      .HasForeignKey(e => e.patient_id);

                //Configuración de negocio
                entity.HasIndex(e => e.claim_number).IsUnique();
                entity.Property(e => e.amount).HasColumnType("decimal(18,2)");

                //Relación con Importaciones
               /* entity.HasOne(e => e.ClaimImport)
                      .WithMany(i => i.Claims) 
                      .HasForeignKey(e => e.claim_import_id)
                      .OnDelete(DeleteBehavior.Restrict);*/
            });
        }
    }
}
