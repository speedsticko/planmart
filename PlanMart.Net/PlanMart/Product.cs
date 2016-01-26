namespace PlanMart
{
    public class Product
    {
        /// <summary>
        /// The unit price of the product.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The weight in pounds of the product.
        /// </summary>
        public decimal Weight { get; }

        /// <summary>
        /// The class of product (alcohol, clothes, food, etc.).
        /// </summary>
        public ProductType Type { get; }

        public Product(decimal price, decimal weight, ProductType type)
        {
            Price = price;
            Weight = weight;
            Type = type;
        }
    }
}