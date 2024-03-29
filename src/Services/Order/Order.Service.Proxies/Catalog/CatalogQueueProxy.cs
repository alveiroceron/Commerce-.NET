﻿using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Options;
using Order.Service.Proxies.Catalog.Commands;
using System.Text;
using System.Text.Json;

namespace Order.Service.Proxies.Catalog
{
    public class CatalogQueueProxy : ICatalogProxy
    {
        private readonly string _connectionString;

        public CatalogQueueProxy(
            IOptions<AzureServiceBus> azure)
        {
            _connectionString = azure.Value.ConnectionString;
        }
        public async Task UpdateStockAsync(ProductInStockUpdateStockCommand command)
        {
            var queueClient = new QueueClient(_connectionString, "order-stock-update");

         

            // Serialize message
            string body = JsonSerializer.Serialize(command);
            var message = new Message(Encoding.UTF8.GetBytes(body));

            // Send the message to the queue
            await queueClient.SendAsync(message);

            // Close
            await queueClient.CloseAsync();
        }

    }
}
