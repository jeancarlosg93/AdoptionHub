using System;
using System.Collections.Generic;
using AdoptionHub.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptionHub.Contexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adoptionapplication> Adoptionapplications { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Fosterassignment> Fosterassignments { get; set; }

    public virtual DbSet<Medicalrecord> Medicalrecords { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Petimage> Petimages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vetappointment> Vetappointments { get; set; }

    public virtual DbSet<Veterinarian> Veterinarians { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Adoptionapplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("adoptionapplications");

            entity.HasIndex(e => e.AdopterId, "adopterId");

            entity.HasIndex(e => e.PetId, "petId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdopterId).HasColumnName("adopterId");
            entity.Property(e => e.ApplicationDate).HasColumnName("applicationDate");
            entity.Property(e => e.ApplicationStatus)
                .HasMaxLength(15)
                .HasColumnName("applicationStatus");
            entity.Property(e => e.PetId).HasColumnName("petId");

            entity.HasOne(d => d.Adopter).WithMany(p => p.Adoptionapplications)
                .HasForeignKey(d => d.AdopterId)
                .HasConstraintName("AdoptionApplications_ibfk_1");

            entity.HasOne(d => d.Pet).WithMany(p => p.Adoptionapplications)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("AdoptionApplications_ibfk_2");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("appointments");

            entity.HasIndex(e => e.CreatedByUserId, "createdByUserId");

            entity.HasIndex(e => e.FosterUserId, "fosterUserId");

            entity.HasIndex(e => e.PetId, "petId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointmentDate");
            entity.Property(e => e.CreatedByUserId).HasColumnName("createdByUserId");
            entity.Property(e => e.FosterUserId).HasColumnName("fosterUserId");
            entity.Property(e => e.PetId).HasColumnName("petId");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.AppointmentCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .HasConstraintName("Appointments_ibfk_1");

            entity.HasOne(d => d.FosterUser).WithMany(p => p.AppointmentFosterUsers)
                .HasForeignKey(d => d.FosterUserId)
                .HasConstraintName("Appointments_ibfk_2");

            entity.HasOne(d => d.Pet).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("Appointments_ibfk_3");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Fosterassignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("fosterassignments");

            entity.HasIndex(e => e.FosterId, "fosterId");

            entity.HasIndex(e => e.PetId, "petId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.FosterId).HasColumnName("fosterId");
            entity.Property(e => e.PetId).HasColumnName("petId");
            entity.Property(e => e.StartDate).HasColumnName("startDate");

            entity.HasOne(d => d.Foster).WithMany(p => p.Fosterassignments)
                .HasForeignKey(d => d.FosterId)
                .HasConstraintName("FosterAssignments_ibfk_1");

            entity.HasOne(d => d.Pet).WithMany(p => p.Fosterassignments)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FosterAssignments_ibfk_2");
        });

        modelBuilder.Entity<Medicalrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("medicalrecords");

            entity.HasIndex(e => e.PetId, "petId");

            entity.HasIndex(e => e.VetId, "vetId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HealthStatus)
                .HasMaxLength(100)
                .HasColumnName("healthStatus");
            entity.Property(e => e.IsNeutered).HasColumnName("isNEUTERED");
            entity.Property(e => e.IsVaccinated).HasColumnName("isVaccinated");
            entity.Property(e => e.PetId).HasColumnName("petId");
            entity.Property(e => e.SpecialNeeds)
                .HasMaxLength(100)
                .HasColumnName("specialNeeds");
            entity.Property(e => e.VetId).HasColumnName("vetId");
            entity.Property(e => e.VisitDate).HasColumnName("visitDate");
            entity.Property(e => e.VisitDescription)
                .HasMaxLength(255)
                .HasColumnName("visitDescription");

            entity.HasOne(d => d.Pet).WithMany(p => p.Medicalrecords)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("MedicalRecords_ibfk_1");

            entity.HasOne(d => d.Vet).WithMany(p => p.Medicalrecords)
                .HasForeignKey(d => d.VetId)
                .HasConstraintName("MedicalRecords_ibfk_2");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pets");

            entity.HasIndex(e => e.FosterParentId, "IX_pets_FosterParentId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdoptionFee)
                .HasPrecision(10, 2)
                .HasColumnName("adoptionFee");
            entity.Property(e => e.Bio)
                .HasMaxLength(255)
                .HasColumnName("bio");
            entity.Property(e => e.Breed)
                .HasMaxLength(25)
                .HasColumnName("breed");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.DateArrived).HasColumnName("dateArrived");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Species)
                .HasMaxLength(3)
                .HasColumnName("species");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Temperament)
                .HasMaxLength(30)
                .HasColumnName("temperament");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.FosterParent).WithMany(p => p.Pets).HasForeignKey(d => d.FosterParentId);
        });

        modelBuilder.Entity<Petimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("petimages");

            entity.HasIndex(e => e.PetId, "petId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("imageUrl");
            entity.Property(e => e.PetId).HasColumnName("petId");

            entity.HasOne(d => d.Pet).WithMany(p => p.Petimages)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("petimages_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastName");
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.UserRole)
                .HasMaxLength(15)
                .HasColumnName("userRole");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Vetappointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vetappointments");

            entity.HasIndex(e => e.FosterId, "FK_VetAppointments_FosterId");

            entity.HasIndex(e => e.PetId, "FK_VetAppointments_PetId");

            entity.HasIndex(e => e.VetId, "FK_VetAppointments_VetId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApptDate).HasColumnName("apptDate");
            entity.Property(e => e.ApptReason)
                .HasMaxLength(200)
                .HasColumnName("apptReason");
            entity.Property(e => e.FosterId).HasColumnName("fosterId");
            entity.Property(e => e.IsFostered).HasColumnName("isFostered");
            entity.Property(e => e.PetId).HasColumnName("petId");
            entity.Property(e => e.VetId).HasColumnName("vetId");

            entity.HasOne(d => d.Foster).WithMany(p => p.Vetappointments)
                .HasForeignKey(d => d.FosterId)
                .HasConstraintName("FK_VetAppointments_FosterId");

            entity.HasOne(d => d.Pet).WithMany(p => p.Vetappointments)
                .HasForeignKey(d => d.PetId)
                .HasConstraintName("FK_VetAppointments_PetId");

            entity.HasOne(d => d.Vet).WithMany(p => p.Vetappointments)
                .HasForeignKey(d => d.VetId)
                .HasConstraintName("FK_VetAppointments_VetId");
        });

        modelBuilder.Entity<Veterinarian>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("veterinarian");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("lastName");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(30)
                .HasColumnName("phoneNumber");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
