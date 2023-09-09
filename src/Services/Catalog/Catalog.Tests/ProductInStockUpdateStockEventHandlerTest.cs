using Catalog.Service.EventHandlers;
using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.EventHandlers.Exceptions;
using Catalog.Tests.Config;
using Microsoft.Extensions.Logging;
using Moq;
using static Catalog.Common.Enums;

namespace Catalog.Tests
{
    [TestClass]
    public class ProductInStockUpdateStockEventHandlerTest
    {
        private static ILogger<ProductInStockUpdateStockEventHandler> GetLogger
        {
            get 
            { 
                    return new Mock<ILogger<ProductInStockUpdateStockEventHandler>>().Object;          
            }     
        }

        [TestMethod]
        public void TryToSubstractStockWhenProductHasStock()
        {
            var context = ApplicationDbContextInMemory.Get();

            var productInStock = 1;
            var producId = 1;

            context.Stocks.Add(new Domain.ProductInStock
            {
                ProducInStockId = productInStock,
                ProductId = producId,
                Stock = 1
            });

            context.SaveChanges();

            var handler = new ProductInStockUpdateStockEventHandler(context, GetLogger);

            handler.Handle(new ProductInStockUpdateStockCommand{ 
                Items = new List<ProductInStockUpdateItem>(){ 
                    new ProductInStockUpdateItem { 
                        ProductId = producId,
                        Stock = 1,
                        Action = ProductInStockAction.Substract
                    }
                }
            },new CancellationToken()).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(ProductInStockUpdateStockCommandException))]
        public void TryToSubstractStockWhenProductHasntStock()
        {
            var context = ApplicationDbContextInMemory.Get();

            var productInStock = 2;
            var producId = 2;

            context.Stocks.Add(new Domain.ProductInStock
            {
                ProducInStockId = productInStock,
                ProductId = producId,
                Stock = 1
            });

            context.SaveChanges();

            var handler = new ProductInStockUpdateStockEventHandler(context, GetLogger);

            try
            {
                handler.Handle(new ProductInStockUpdateStockCommand
                {
                    Items = new List<ProductInStockUpdateItem>(){
                    new ProductInStockUpdateItem {
                        ProductId = producId,
                        Stock = 2,
                        Action = ProductInStockAction.Substract
                    }
                }
                }, new CancellationToken()).Wait();
            }
            catch (AggregateException ae)
            {
                var exception = ae.GetBaseException();

                if (exception is ProductInStockUpdateStockCommandException)
                {
                    throw new ProductInStockUpdateStockCommandException(exception?.InnerException?.Message);
                }
            }
        }

        [TestMethod]
        public void TryToAddStockWhenProductExists()
        {
            var context = ApplicationDbContextInMemory.Get();

            var productInStock = 3;
            var producId = 3;

            context.Stocks.Add(new Domain.ProductInStock
            {
                ProducInStockId = productInStock,
                ProductId = producId,
                Stock = 1
            });

            context.SaveChanges();

            var handler = new ProductInStockUpdateStockEventHandler(context, GetLogger);

            handler.Handle(new ProductInStockUpdateStockCommand
            {
                Items = new List<ProductInStockUpdateItem>(){
                    new ProductInStockUpdateItem {
                        ProductId = producId,
                        Stock = 2,
                        Action = ProductInStockAction.Add
                    }
                }
            }, new CancellationToken()).Wait();

            var stockInDb = context.Stocks.Single(x => x.ProductId == producId).Stock;
            Assert.AreEqual(stockInDb, 3);           
        }

        [TestMethod]
        public void TryToAddStockWhenProductNotExists()
        {
            var context = ApplicationDbContextInMemory.Get();

            var producId = 4;

            context.SaveChanges();

            var handler = new ProductInStockUpdateStockEventHandler(context, GetLogger);

            handler.Handle(new ProductInStockUpdateStockCommand
            {
                Items = new List<ProductInStockUpdateItem>(){
                    new ProductInStockUpdateItem {
                        ProductId = producId,
                        Stock = 2,
                        Action = ProductInStockAction.Add
                    }
                }
            }, new CancellationToken()).Wait();

            var stockInDb = context.Stocks.Single(x => x.ProductId == producId).Stock;
            Assert.AreEqual(stockInDb, 2);
        }
    }
}