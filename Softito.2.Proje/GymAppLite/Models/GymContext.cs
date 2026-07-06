using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GymAppLite.Models;

public partial class GymContext : IdentityDbContext
{
    public GymContext()
    {
    }

    public GymContext(DbContextOptions<GymContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<ProgressLog> ProgressLogs { get; set; }

    public virtual DbSet<SubscriptionType> SubscriptionTypes { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=GymAppLite;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Members__3214EC07F38CCB4B");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.SubscriptionTypeId).HasColumnName("SubscriptionTypeID");
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");

            entity.HasOne(d => d.SubscriptionType).WithMany(p => p.Members)
                .HasForeignKey(d => d.SubscriptionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Members_SubscriptionTypes");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Members)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Members_Trainers");
        });

        modelBuilder.Entity<ProgressLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Progress__3214EC079B010FB9");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Member).WithMany(p => p.ProgressLogs)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("FK_ProgressLogs_Members");
        });

        modelBuilder.Entity<SubscriptionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC076CABBAA9");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trainers__3214EC070AEADD0A");

            entity.Property(e => e.Expertise).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
