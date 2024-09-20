using System;
using Shared.Dtos;
using Shared.Models;

namespace ProductAPI.Repositories
{
	public interface IProduct
	{
		Task<ServiceResponse> AddProductAsync(Product product);
		Task<List<Product>> GetAllProductsAsync();
	}
}

