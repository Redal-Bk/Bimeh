using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Bimeh.Domain.Entities;

public partial class BimehContext : DbContext
{
    public BimehContext()
    {
    }

    public BimehContext(DbContextOptions<BimehContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InsuranceCoverage> InsuranceCoverages { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<SumInsuranceItem> SumInsuranceItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InsuranceCoverage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Insuranc__3214EC07392A3F65");

            entity.ToTable("InsuranceCoverage");

            entity.Property(e => e.Title).HasMaxLength(30);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Requests__3213E83F4CF25573");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.RequestTitle).HasMaxLength(500);

            entity.HasOne(d => d.InsuranceCoverage).WithMany(p => p.Requests)
                .HasForeignKey(d => d.InsuranceCoverageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requests__Insura__412EB0B6");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Requests__UserId__403A8C7D");
        });

        modelBuilder.Entity<SumInsuranceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SumInsur__3214EC0769C53767");

            entity.HasOne(d => d.Insurance).WithMany(p => p.SumInsuranceItems)
                .HasForeignKey(d => d.InsuranceId)
                .HasConstraintName("FK__SumInsura__Insur__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.SumInsuranceItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SumInsura__UserI__440B1D61");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC075FBC9659");

            entity.Property(e => e.Password).HasMaxLength(30);
            entity.Property(e => e.Username).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
