using BusinessObjects;
using ProductWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.impl;
using BusinessObjects.Models;

namespace ProductWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderRepository repository = new OrderRepository();
        private IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        private IProductRepository ProductRepository = new ProductRepository();

		[HttpGet]
		public ActionResult<IEnumerable<Order>> GetOrders() => Ok(repository.GetOrders());

		[HttpGet("customer/{id}")]
        public ActionResult<IEnumerable<Order>> GetAllOrdersByCustomerId(string id)
        {
            var listOrder = repository.GetAllOrdersByCustomerId(id);
            foreach (var o in listOrder)
            {
                var orderDetails = orderDetailRepository.GetOrderDetailsByOrderId(o.OrderId);
				o.OrderDetails = orderDetails;
			}
            return Ok(listOrder);
        }

		[HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id) {
            var order = repository.FindOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
			var orderDetails = orderDetailRepository.GetOrderDetailsByOrderId(id);
			order.OrderDetails = orderDetails;
			return Ok(order);
        }

		[HttpGet("customer/detail/{id}")]
		public ActionResult<Order> GetOrderDetailById(int id)
		{
			var order = repository.FindOrderById(id);
			if (order == null)
			{
				return NotFound();
			}
			var orderDetails = orderDetailRepository.GetOrderDetailsByOrderId(id);
			order.OrderDetails = orderDetails;
			return Ok(order);
		}

        [HttpPost]
        public ActionResult<Order> PostOrder(PostOrder postOrder)
        {
            foreach (var od in postOrder.OrderDetails)
            { 
                var fb = ProductRepository.GetProductById(od.ProductID);
                if (fb == null)
                {
                    return NotFound();
                }
                if (fb.UnitInStock < od.Quantity)
                {
                    return BadRequest();
                }
            }
            var order = new Order
            {
                OrderDate = postOrder.OrderDate,
                ShippedDate = DateTime.Now,
                Freight = postOrder.Freight,
                MemberId = postOrder.CustomerID
            };
            var savedOrder = repository.SaveOrder(order);
            foreach (var od in postOrder.OrderDetails)
            {
                var fb = ProductRepository.GetProductById(od.ProductID);
                fb.UnitInStock -= od.Quantity;
                var orderDetail = new OrderDetail
                {
                    ProductId = od.ProductID,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    OrderId = savedOrder.OrderId
                };
                ProductRepository.UpdateProduct(fb);
                orderDetailRepository.SaveOrderDetail(orderDetail);
            }
            return Ok(savedOrder);
        }

        [HttpPut("shipped/{id}")]
        public IActionResult PutOrderShipped(int id)
        {
            var oTmp = repository.FindOrderById(id);
            if (oTmp == null)
            {
                return NotFound();
            }
            oTmp.ShippedDate = DateTime.Now;
            repository.UpdateOrder(oTmp);
            return NoContent();
        }

		[HttpPut("cancel/{id}")]
        public IActionResult PutOrderCancel(int id)
        {
            var oTmp = repository.FindOrderById(id);
            if (oTmp == null)
            {
                return NotFound();
            }
            repository.UpdateOrder(oTmp);
            var orderDetails = orderDetailRepository.GetOrderDetailsByOrderId(id);
            foreach (var od in orderDetails)
            {
                var fb = ProductRepository.GetProductById(od.ProductId);
                fb.UnitInStock += od.Quantity;
                ProductRepository.UpdateProduct(fb);
            }
            return NoContent();
        }
    }
}
