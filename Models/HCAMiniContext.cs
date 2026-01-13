using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Models;

public partial class HCAMiniContext : DbContext
{
    public HCAMiniContext()
    {
    }

    public HCAMiniContext(DbContextOptions<HCAMiniContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<LabOrder> LabOrders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- 1. Map Patient to [Healthcare].[Patient] ---
        modelBuilder.Entity<Patient>(entity =>
        {
            // THIS LINE FIXES YOUR ERROR:
            entity.ToTable("Patient", "Healthcare");

            entity.HasKey(e => e.PatientId);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        // --- 2. Map Appointment to [Healthcare].[Appointment] ---
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointment", "Healthcare");

            entity.HasKey(e => e.AppointmentId);
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Scheduled");
            entity.Property(e => e.DoctorName).HasMaxLength(50);

            // Relationship
            entity.HasOne(d => d.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull); // Use ClientSetNull to match our "Safe Delete" logic
        });

        // --- 3. Map LabOrder to [Healthcare].[LabOrder] ---
        modelBuilder.Entity<LabOrder>(entity =>
        {
            entity.ToTable("LabOrder", "Healthcare");

            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.TestName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.OrderDate).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Pending");

            entity.HasOne(d => d.Appointment)
                .WithMany()
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --- 4. Map AuditLog to [Healthcare].[AuditLog] ---
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLog", "Healthcare");
            entity.HasKey(e => e.LogId);
        });
    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
