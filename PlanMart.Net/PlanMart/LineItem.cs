namespace PlanMart
{
    /// <summary>
    /// Represents adjustments made by the processor to indicate taxes, shipping costs, and awarded rewards points.
    /// </summary>
    public class LineItem
    {
        /// <summary>
        /// The type of line item (tax, shipping, rewards points)
        /// </summary>
        public LineItemType Type { get; }

        /// <summary>
        /// The amount of taxes, shipping costs, or rewards points added to the order.
        /// </summary>
        public decimal Amount { get; }

        public LineItem(LineItemType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}