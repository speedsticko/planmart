namespace PlanMart
{
    public class Product
    {
        public decimal Price { get; }
        public ProductType Type { get; }

        public Product(decimal price, ProductType type)
        {
            Price = price;
            Type = type;
        }
    }
}