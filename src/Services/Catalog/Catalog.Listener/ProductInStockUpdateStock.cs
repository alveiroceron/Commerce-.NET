namespace Catalog.Listener
{
    public enum ProductInStockAction
    {
        Add,
        Substract
    }

    public class ProductInStockUpdateStock
    {
        public IEnumerable<ProductInStockUpdateItem> Items { get; set; } = new List<ProductInStockUpdateItem>();
    }
    public class ProductInStockUpdateItem
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
        public ProductInStockAction Action { get; set; }
    }
}
