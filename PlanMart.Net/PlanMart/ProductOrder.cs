namespace PlanMart
{
    /// <summary>
    /// Contained by an Order and represents a single product type and the quantity of that product that was ordered.
    /// </summary>
    public class ProductOrder 
    {
        /// <summary>
        /// The product that was ordered.
        /// </summary>
        public Product Product { get; }

        /// <summary>
        /// The number of the product that was ordered.
        /// </summary>
        public int Quantity { get; }

        public ProductOrder(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}