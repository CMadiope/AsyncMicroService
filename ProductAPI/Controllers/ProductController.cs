using Microsoft.AspNetCore.Mvc;
using ProductAPI.Repositories;
using Shared.Dtos;
using Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productInterface;

        public ProductController(IProduct productInterface)
        {
            _productInterface = productInterface;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct(Product product)
        {
            var response = await _productInterface.AddProductAsync(product);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts() => await _productInterface.GetAllProductsAsync();
    }
}

