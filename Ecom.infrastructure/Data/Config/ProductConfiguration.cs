using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(180);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new Product { Id = 1, Name = "Samsung Galaxy S21", Description = "Samsung Galaxy S21 5G", Price = 799.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Apple iPhone 12", Description = "Apple iPhone 12 Pro Max", Price = 1099.99m, CategoryId = 1 },
                new Product { Id = 3, Name = "Dell XPS 13", Description = "Dell XPS 13 9310", Price = 1299.99m, CategoryId = 1 },
                new Product { Id = 4, Name = "Nike Air Max", Description = "Nike Air Max 270", Price = 150.00m, CategoryId = 2 },
                new Product { Id = 5, Name = "Adidas Superstar", Description = "Adidas Superstar", Price = 80.00m, CategoryId = 2 },
                new Product { Id = 6, Name = "Levi's Jeans", Description = "Levi's 501 Original Fit Jeans", Price = 59.99m, CategoryId = 2 },
                new Product { Id = 7, Name = "The Alchemist", Description = "The Alchemist by Paulo Coelho", Price = 12.99m, CategoryId = 3 },
                new Product { Id = 8, Name = "The Great Gatsby", Description = "The Great Gatsby by F. Scott Fitzgerald", Price = 9.99m, CategoryId = 3 },
                new Product { Id = 9, Name = "To Kill a Mockingbird", Description = "To Kill a Mockingbird by Harper Lee", Price = 14.99m, CategoryId = 3 }
            );
        }
    }
}
