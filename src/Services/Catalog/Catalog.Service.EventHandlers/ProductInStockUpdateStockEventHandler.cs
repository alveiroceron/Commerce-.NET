using Catalog.Domain;
using Catalog.Persistence.Database;
using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.EventHandlers.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Catalog.Common.Enums;

namespace Catalog.Service.EventHandlers
{
    public class ProductInStockUpdateStockEventHandler : INotificationHandler<ProductInStockUpdateStockCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductInStockUpdateStockEventHandler> _logger;

        public ProductInStockUpdateStockEventHandler(
            ApplicationDbContext context,
            ILogger<ProductInStockUpdateStockEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(ProductInStockUpdateStockCommand notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--- ProductInStockUpdateStockCommand started.");

            var products = notification.Items.Select(x => x.ProductId);
            var stocks = await _context.Stocks.Where(x => products.Contains(x.ProductId)).ToListAsync();

            _logger.LogInformation("--- Retrieve products from database.");

            foreach (var item in notification.Items)
            {

                var entry = stocks.SingleOrDefault(x => x.ProductId == item.ProductId);

                if (item.Action == ProductInStockAction.Substract)
                {
                    if (entry == null || item.Stock > entry.Stock)
                    {
                        _logger.LogError($"--- Product {entry.ProductId} - does not have enough stock.");
                        throw new ProductInStockUpdateStockCommandException($"--- Product {entry.ProductId} - does not have enough stock.");
                    }
                 
                    entry.Stock -= item.Stock;
                    _logger.LogInformation($"--- Product {entry.ProductId} - it's stock was substracted and its new stock is {entry.Stock}");
                }
                else
                {
                    if (entry == null)
                    {
                        entry = new ProductInStock
                        {
                            ProductId = item.ProductId
                        };

                        await _context.AddAsync(entry);
                        _logger.LogInformation($"--- New stock record was created for  {entry.ProductId} because did not exists before ");

                    }
                    entry.Stock += item.Stock;
                    _logger.LogInformation($"--- Product {entry.ProductId} - it's stock was increased and its new stock is {entry.Stock}");
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("--- ProductInStockUpdateStockCommand ended.");
        }
    }
}
