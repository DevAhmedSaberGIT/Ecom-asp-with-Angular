﻿// <auto-generated />
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ecom.infrastructure.DataMigration
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250408120234_updateproduct")]
    partial class updateproduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ecom.Core.Entities.Product.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Electronic gadgets",
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Clothing items",
                            Name = "Clothing"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Books and novels",
                            Name = "Books"
                        });
                });

            modelBuilder.Entity("Ecom.Core.Entities.Product.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Photos");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            ImageName = "TEST",
                            ProductId = 1
                        });
                });

            modelBuilder.Entity("Ecom.Core.Entities.Product.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(180)
                        .HasColumnType("nvarchar(180)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("NewPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OldPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "Samsung Galaxy S21 5G",
                            Name = "Samsung Galaxy S21",
                            NewPrice = 799.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Description = "Apple iPhone 12 Pro Max",
                            Name = "Apple iPhone 12",
                            NewPrice = 1099.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            Description = "Dell XPS 13 9310",
                            Name = "Dell XPS 13",
                            NewPrice = 1299.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            Description = "Nike Air Max 270",
                            Name = "Nike Air Max",
                            NewPrice = 150.00m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 2,
                            Description = "Adidas Superstar",
                            Name = "Adidas Superstar",
                            NewPrice = 80.00m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 2,
                            Description = "Levi's 501 Original Fit Jeans",
                            Name = "Levi's Jeans",
                            NewPrice = 59.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 3,
                            Description = "The Alchemist by Paulo Coelho",
                            Name = "The Alchemist",
                            NewPrice = 12.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 3,
                            Description = "The Great Gatsby by F. Scott Fitzgerald",
                            Name = "The Great Gatsby",
                            NewPrice = 9.99m,
                            OldPrice = 0m
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 3,
                            Description = "To Kill a Mockingbird by Harper Lee",
                            Name = "To Kill a Mockingbird",
                            NewPrice = 14.99m,
                            OldPrice = 0m
                        });
                });

            modelBuilder.Entity("Ecom.Core.Entities.Product.Photo", b =>
                {
                    b.HasOne("Ecom.Core.Entities.Product.Product", null)
                        .WithMany("photos")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Ecom.Core.Entities.Product.Product", b =>
                {
                    b.HasOne("Ecom.Core.Entities.Product.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Ecom.Core.Entities.Product.Product", b =>
                {
                    b.Navigation("photos");
                });
#pragma warning restore 612, 618
        }
    }
}
