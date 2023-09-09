using Catalog.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Database.Configuration
{
    public class ProductInStockConfiguration
    {
        public ProductInStockConfiguration(EntityTypeBuilder<ProductInStock> entityBuilder)
        {
            entityBuilder.HasKey(x => x.ProducInStockId);

            var products = new List<ProductInStock>();
            var random = new Random();

            for (int i = 1; i < 100; i++)
            {
                products.Add(new ProductInStock
                {
                    ProducInStockId = i,
                    ProductId = i,
                    Stock = random.Next(0, 20)

                });
            }

            entityBuilder.HasData(products);

        }
    }
}
