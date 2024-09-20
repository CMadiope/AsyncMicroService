using System;
using Shared.Dtos;
using Shared.Models;

namespace OrderAPI.Repositories
{
	public interface IOrder
	{
		Task<ServiceResponse> AddOrderAsync(Order order);
		Task<List<Order>> GetAllOrdersAsync();
		Task<OrderSummary> GetOrderSummaryAsync();
	}
}

