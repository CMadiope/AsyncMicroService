using System;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using Shared.Dtos;
using Shared.Models;

namespace ProductAPI.Repositories
{
	public class ProductRepository : IProduct
	{
        private readonly ProductDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductRepository(ProductDbContext context, IPublishEndpoint publishEndpoint)
		{
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ServiceResponse> AddProductAsync(Product product)
        {
           _context.Products.Add(product);
            await _context.SaveChangesAsync();
            await _publishEndpoint.Publish(product);
            return new ServiceResponse(true, "Product added successfully.");
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products;
        }
    }
}

