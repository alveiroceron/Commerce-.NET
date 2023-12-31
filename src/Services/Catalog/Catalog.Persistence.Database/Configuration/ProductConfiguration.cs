﻿using Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Database.Configuration
{
    public class ProductConfiguration
    {
        public ProductConfiguration(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(x => x.ProductId);
            entityBuilder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            entityBuilder.Property(x => x.Description).IsRequired().HasMaxLength(500);


            var products = new List<Product>();
            var random = new Random();

            for (int i = 1; i < 100; i++)
            {
                products.Add(new Product
                {
                    ProductId = i,
                    Name = $"Product {i}",
                    Description = $"Description for ptoduct {i}",
                    Price = random.Next(100,1000)
                });
            }

            entityBuilder.HasData(products);
        }
    }
}
