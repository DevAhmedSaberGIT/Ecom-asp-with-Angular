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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic gadgets" },
                new Category { Id = 2, Name = "Clothing", Description = "Clothing items" },
                new Category { Id = 3, Name = "Books", Description = "Books and novels" }
            );
        }
    }
}
