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
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCC25C1001C1");

            entity.ToTable("Appointment", "Healthcare", tb => tb.HasTrigger("trg_Appointment_Audit"));

            entity.Property(e => e.Status).HasDefaultValue("Scheduled");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments).HasConstraintName("FK_Appointment_Patient");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E54864888F4FD9F");

            entity.Property(e => e.LogDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LabOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__LabOrder__C3905BCF94261420");

            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.Appointment).WithMany(p => p.LabOrders).HasConstraintName("FK_LabOrder_Appointment");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patient__970EC3661E5F4204");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
