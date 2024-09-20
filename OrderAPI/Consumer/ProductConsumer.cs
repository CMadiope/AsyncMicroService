using System;
using MassTransit;
using OrderAPI.Data;
using Shared.Models;

namespace OrderAPI.Consumer
{
	public class ProductConsumer : IConsumer<Product>
	{
        private readonly OrderDbContext _context;

        public ProductConsumer(OrderDbContext context)
		{
            _context = context;
        }

        public async Task Consume(ConsumeContext<Product> context)
        {
            _context.Products.Add(context.Message);
            await _context.SaveChangesAsync();
        }
    }
}

