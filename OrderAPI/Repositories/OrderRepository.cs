
using System.Text;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using Shared.Dtos;
using Shared.Models;

namespace OrderAPI.Repositories
{
	public class OrderRepository : IOrder
	{
        private readonly OrderDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderRepository(OrderDbContext context, IPublishEndpoint publishEndpoint)
		{
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ServiceResponse> AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var orderSummary = await GetOrderSummaryAsync();
            string content = BuildOrderEmailBody(orderSummary.Id,orderSummary.ProductName, orderSummary.ProductPrice, orderSummary.Quantity, orderSummary.TotalAmount, orderSummary.Date );
            await _publishEndpoint.Publish(new EmailDto("Order Information", content));
            await ClearOrderTable();
            return new ServiceResponse(true, "Order placed successfully");
        }

        

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task<OrderSummary> GetOrderSummaryAsync()
        {
            var order = await _context.Orders.FirstOrDefaultAsync();
            var products = await _context.Products.ToListAsync();
            var productInfo = products.Find(x => x.Id == order!.ProductId);
            return new OrderSummary
                (
                    order!.Id,
                    order.ProductId,
                    productInfo!.Name,
                    productInfo.Price,
                    order.Quantity,
                    order.Quantity * productInfo.Price,
                    order.Date
                );
        }

        private string BuildOrderEmailBody(int orderId, string productName, decimal price, int orderQuantity, decimal totalAmount, DateTime date)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h1><strong>Order Information </strong></h1>");
            sb.AppendLine($"<p><strong>Order ID: </strong> {orderId}</p>");

            sb.AppendLine("<h2>Order Item: </h2>");
            sb.AppendLine("<ul>");
            sb.AppendLine($"<li>Name: {productName} </li>");
            sb.AppendLine($"<li>Price: {price} </li>");
            sb.AppendLine($"<li>Quantity: {orderQuantity} </li>");
            sb.AppendLine($"<li>Total Amount: {totalAmount} </li>");
            sb.AppendLine($"<li> Date Ordered: {date} </li>");


            sb.AppendLine("</ul>");

            sb.AppendLine("<p>Thank you for your order </p>");
            return sb.ToString();
        }

        private async Task ClearOrderTable()
        {
            _context.Orders.Remove(await _context.Orders.FirstOrDefaultAsync()
                );
            await _context.SaveChangesAsync();
        }
    }

}

