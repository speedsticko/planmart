namespace PlanMart
{
    public interface IOrderProcessor
    {
        bool ProcessOrder(Order order);
    }
}