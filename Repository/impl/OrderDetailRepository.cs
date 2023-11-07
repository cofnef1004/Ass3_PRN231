using BusinessObjects;
using BusinessObjects.Models;
using DataAccess;

namespace Repositories.impl
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public void SaveOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.SaveOrderDetail(orderDetail);
        public OrderDetail GetOrderDetailByOrderIdAndProductId(int orderId, int ProductId) => OrderDetailDAO.FindOrderDetailByOrderIdAndProductId(orderId, ProductId);
        public List<OrderDetail> GetOrderDetails() => OrderDetailDAO.GetOrderDetails();
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId) => OrderDetailDAO.FindAllOrderDetailsByOrderId(orderId);
        public void UpdateOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.UpdateOrderDetail(orderDetail);
        public void DeleteOrderDetail(OrderDetail orderDetail) => OrderDetailDAO.DeleteOrderDetail(orderDetail);
    }
}
