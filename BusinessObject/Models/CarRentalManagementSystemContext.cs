using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.Models;

public partial class CarRentalManagementSystemContext : DbContext
{
    public CarRentalManagementSystemContext()
    {
    }

    public CarRentalManagementSystemContext(DbContextOptions<CarRentalManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarRental> CarRentals { get; set; }

    public virtual DbSet<CarStatus> CarStatuses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<HistoryCarRental> HistoryCarRentals { get; set; }

    public virtual DbSet<RankLevelCustomer> RankLevelCustomers { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;database=CarRentalManagementSystem;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.LicensePlates).HasName("PK__Car__AE763D17061E67F1");

            entity.ToTable("Car");

            entity.Property(e => e.LicensePlates).HasMaxLength(50);
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.CarName).HasMaxLength(80);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Fuel).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RentalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.CarStatus).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarStatusId)
                .HasConstraintName("FK__Car__CarStatusId__2E1BDC42");
        });

        modelBuilder.Entity<CarRental>(entity =>
        {
            entity.HasKey(e => e.RentalId).HasName("PK__CarRenta__9700594349ED6A0D");

            entity.ToTable("CarRental");

            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.LicensePlates).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Customer).WithMany(p => p.CarRentals)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CarRental__Custo__31EC6D26");

            entity.HasOne(d => d.LicensePlatesNavigation).WithMany(p => p.CarRentals)
                .HasForeignKey(d => d.LicensePlates)
                .HasConstraintName("FK__CarRental__Licen__32E0915F");

            entity.HasOne(d => d.Staff).WithMany(p => p.CarRentals)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__CarRental__Staff__30F848ED");
        });

        modelBuilder.Entity<CarStatus>(entity =>
        {
            entity.HasKey(e => e.CarStatusId).HasName("PK__CarStatu__4A328CC668EC71EB");

            entity.ToTable("CarStatus");

            entity.Property(e => e.CarStatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D84E7D2CFD");

            entity.ToTable("Customer");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.RankLevelNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.RankLevel)
                .HasConstraintName("FK__Customer__RankLe__29572725");
        });

        modelBuilder.Entity<HistoryCarRental>(entity =>
        {
            entity.HasKey(e => e.HistoryCarRentalId).HasName("PK__HistoryC__897732B20130C528");

            entity.ToTable("HistoryCarRental");

            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Rental).WithMany(p => p.HistoryCarRentals)
                .HasForeignKey(d => d.RentalId)
                .HasConstraintName("FK__HistoryCa__Renta__3B75D760");
        });

        modelBuilder.Entity<RankLevelCustomer>(entity =>
        {
            entity.HasKey(e => e.RankLevelId).HasName("PK__RankLeve__B7D67F90916BCEF7");

            entity.ToTable("RankLevelCustomer");

            entity.Property(e => e.RankLevelName).HasMaxLength(50);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB173068E623");

            entity.HasIndex(e => e.Email, "UQ__Staff__A9D10534B8A84DFA").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StaffName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
