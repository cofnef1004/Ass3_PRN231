using BusinessObjects;
using BusinessObjects.Models;

namespace Repositories
{
    public interface IOrderRepository
    {
        Order SaveOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(Order order);
		List<Order> GetOrders();

		List<OrderDetail> GetOrderDetails(int orderId);
        List<Order> GetAllOrdersByCustomerId(string memId);

        Order FindOrderById(int orderId);

	}
}
