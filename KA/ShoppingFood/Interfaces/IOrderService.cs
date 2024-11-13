using ShoopingFood.Models;

namespace ShoopingFood.Interfaces
{
    public interface IOrderService
    {
        Task<Receipt?> ProcessOrder(ListOrderItem orderItems);
    }

}
