using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Lab03WebApiOrdenesCompras.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=BRAYAN;Database=OrdenesCompra_dev_db;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CUSTOMER");

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(40);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(40);

            entity.Property(e => e.City)
                .HasMaxLength(40);

            entity.Property(e => e.Country)
                .HasMaxLength(40);

            entity.Property(e => e.Phone)
                .HasMaxLength(20);

            // ✅ Configuración del campo Email
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            // ✅ Configuración del campo FechaNacimiento (tipo DATE)
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .IsRequired(false);

            // Índices
            entity.HasIndex(e => new { e.LastName, e.FirstName }, "IndexCustomerName");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ORDER");

            entity.Property(e => e.OrderNumber)
                .HasMaxLength(10);

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(12, 2)")
                .HasDefaultValue(0m);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER_REFERENCE_CUSTOMER");

            // Índices
            entity.HasIndex(e => e.CustomerId, "IndexOrderCustomerId");
            entity.HasIndex(e => e.OrderDate, "IndexOrderOrderDate");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ORDERITEM");

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(12, 2)");

            entity.Property(e => e.Quantity)
                .HasDefaultValue(1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERITE_REFERENCE_ORDER");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERITE_REFERENCE_PRODUCT");

            // Índices
            entity.HasIndex(e => e.OrderId, "IndexOrderItemOrderId");
            entity.HasIndex(e => e.ProductId, "IndexOrderItemProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PRODUCT");

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Package)
                .HasMaxLength(30);

            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(12, 2)")
                .HasDefaultValue(0m);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_REFERENCE_SUPPLIER");

            // Índices
            entity.HasIndex(e => e.ProductName, "IndexProductName");
            entity.HasIndex(e => e.SupplierId, "IndexProductSupplierId");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SUPPLIER");

            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(40);

            entity.Property(e => e.ContactName)
                .HasMaxLength(50);

            entity.Property(e => e.ContactTitle)
                .HasMaxLength(40);

            entity.Property(e => e.City)
                .HasMaxLength(40);

            entity.Property(e => e.Country)
                .HasMaxLength(40);

            entity.Property(e => e.Phone)
                .HasMaxLength(30);

            entity.Property(e => e.Fax)
                .HasMaxLength(30);

            // ✅ NUEVO: Configuración del campo Email para Supplier
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsRequired(false);

            // Índices
            entity.HasIndex(e => e.CompanyName, "IndexSupplierName");
            entity.HasIndex(e => e.Country, "IndexSupplierCountry");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}