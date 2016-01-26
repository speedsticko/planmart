using System;
using System.Collections.Generic;

namespace PlanMart
{
    public class Order
    {
        /// <summary>
        /// The customer who placed the order.
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// The two-letter region where the order should be shipped to.
        /// </summary>
        public string ShippingRegion { get; }

        /// <summary>
        /// An enum describing the method of payment for the order.
        /// </summary>
        public PaymentMethod PaymentMethod { get; }

        /// <summary>
        /// The date and time in UTC when the order was placed.
        /// </summary>
        public DateTime Placed { get; }

        /// <summary>
        /// A list of items representing one or more products and the quantity of each.
        /// </summary>
        public List<ProductOrder> Items { get; } = new List<ProductOrder>();

        /// <summary>
        /// A list of line items that represent adjustments to the order by the processor (tax, shipping, etc.)
        /// </summary>
        public List<LineItem> LineItems { get; } = new List<LineItem>();

        public Order(Customer customer, string shippingRegion, PaymentMethod paymentMethod, DateTime placed)
        {
            Customer = customer;
            ShippingRegion = shippingRegion;
            PaymentMethod = paymentMethod;
            Placed = placed;
        }
    }
}