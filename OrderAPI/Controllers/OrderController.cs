using Microsoft.AspNetCore.Mvc;
using OrderAPI.Repositories;
using Shared.Dtos;
using Shared.Models;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderInterface;

        public OrderController(IOrder orderInterface)
        {
            _orderInterface = orderInterface;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddOrder(Order order)
        {
            var response = await _orderInterface.AddOrderAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}

