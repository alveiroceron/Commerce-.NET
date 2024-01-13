using Microsoft.Azure.ServiceBus;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Catalog.Listener
{
    class Program
    {
        // Connection String for the namespace can be obtained from the Azure portal under the 
        // 'Shared Access policies' section.
        const string ServiceBusConnectionString = "Endpoint=sb://comerce-queue.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+SKBXsMEkGvGSz5rIHn/amtU0r/4sx3DD+ASbEJHgO8=";
        const string QueueName = "order-stock-update";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine($"Catalog Listener is running ..");
            Console.WriteLine("======================================================");

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await queueClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the MessageHandler Options in terms of exception handling, number of concurrent messages to deliver etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of Concurrent calls to the callback `ProcessMessagesAsync`, set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that will process messages
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var productInStock = JsonSerializer.Deserialize<ProductInStockUpdateStock>(Encoding.UTF8.GetString(message.Body));
            await UpdateStockAsync(productInStock);

            // Complete the message so that it is not received again.
            // This can be done only if the queueClient is created in ReceiveMode.PeekLock mode (which is default).
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the queueClient has already been closed.
            // If queueClient has already been Closed, you may chose to not call CompleteAsync() or AbandonAsync() etc. calls 
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }


        static async Task UpdateStockAsync(ProductInStockUpdateStock model)
        {
            var content = new StringContent(
             JsonSerializer.Serialize(model),
             Encoding.UTF8,
             "application/json"
            );

            using (HttpClient httpClient = new HttpClient())
            {
                // Set the base address of the API 
                httpClient.BaseAddress = new Uri("http://localhost:20000/");

                try
                {
                    // Send a PUT request
                    HttpResponseMessage response = await httpClient.PutAsync("v1/stocks", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and display the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("HTTP request error: " + e.Message);
                }
            }
          
            //var request = await _httpClient.PutAsync(_apiUrls.CatalogUrl + "v1/stocks", content);
            //request.EnsureSuccessStatusCode();
        }

    }
}

