namespace PlanMart.Processors
{
    /// <summary>
    /// Your implementation of IOrderProcessor should go here.
    /// </summary>
    public class PlanMartOrderProcessor : IOrderProcessor
    {
        public bool ProcessOrder(Order order)
        {
            return true;
        }
    }
}