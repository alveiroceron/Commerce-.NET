using MediatR;
using Microsoft.Extensions.Logging;
using Order.Persistence.Database;
using Order.Service.EventHandler.Commands;

namespace Order.Service.EventHandler
{
    public class OrderCreateEventHandler : INotificationHandler<OrderCreateCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderCreateEventHandler> _logger;

        public OrderCreateEventHandler(
            ApplicationDbContext context,
            ILogger<OrderCreateEventHandler> logger)
        {
                _context = context;
            _logger = logger;
        }

        public async Task Handle(OrderCreateCommand notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("--- New order creation started");

            var entry = new Domain.Order();

            using (var trx = await _context.Database.BeginTransactionAsync())
            {
                // 01. Prepare detail
                _logger.LogInformation("--- Preparing detail");
                PrepareDetail(entry, notification);

                // 02. Prepare header
                _logger.LogInformation("--- Preparing header");
                PrepareHeader(entry, notification);

                // 03 Create order
                _logger.LogInformation("--- Creating order");
                await _context.AddAsync(entry);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"--- Order {entry.OrderId} was created");

                // 04 Update Stocks
                _logger.LogInformation("--- Updating stocks");

                await trx.CommitAsync();
            }


        }


        private void PrepareDetail(Domain.Order entry, OrderCreateCommand notification)
        {
            entry.Items = notification.Items.Select(x => new Domain.OrderDetail
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.Price,
                Total = x.Price * x.Quantity
            }).ToList();
        }

        private void PrepareHeader(Domain.Order entry, OrderCreateCommand notification)
        {
            //Header information
            entry.Status = Common.Enums.OrderStatus.Pending;
            entry.PaymentType = notification.PaymentType;
            entry.ClientId = notification.ClientId;
            entry.CreatedAt = DateTime.UtcNow;

            // Sum
            entry.Total = entry.Items.Sum(x => x.Total);
        }

    }
}