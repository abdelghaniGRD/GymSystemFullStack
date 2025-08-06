using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GymSystem.Models;

public partial class GymSystemContext : IdentityDbContext<ApiUser>
{

	

	public GymSystemContext()
    {
       
    }


    public GymSystemContext(DbContextOptions<GymSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.ToTable("attendances");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChekinTime)
                .HasColumnType("datetime")
                .HasColumnName("chekinTime");
            entity.Property(e => e.MemberId).HasColumnName("memberId");

            entity.HasOne(d => d.Member).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_attendances_Members");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Member");

            entity.HasIndex(e => e.AspNetUserId, "IX_Members_aspNetUserId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Addresse)
                .HasMaxLength(50)
                .HasColumnName("addresse");

            entity.Property(e => e.AspNetUserId).HasColumnName("aspNetUserId");

            entity.HasOne(e => e.ApiUser)
                    .WithMany(p => p.Members)
                    .HasForeignKey(f => f.AspNetUserId)
                    .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Birthday).HasColumnName("birthday");

            entity.Property(e => e.IdNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("idNumber");

            entity.Property(e => e.JoinDate).HasColumnName("joinDate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.Phone).HasColumnName("phone");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("plans");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AspNetUser)
                .HasMaxLength(50)
                .HasColumnName("aspNetUser");
            entity.Property(e => e.DurationInMonths).HasColumnName("durationInMonths");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.MemberId).HasColumnName("memberId");
            entity.Property(e => e.PlanId).HasColumnName("planId");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Member).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_subscriptions_Members");

            entity.HasOne(d => d.Plan).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_subscriptions_plans");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
