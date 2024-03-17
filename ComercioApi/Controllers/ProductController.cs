using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComercioApi.Context;
using ComercioApi.Models;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ComercioApi.Services.ProductService;
using ComercioApi.Services.ProductService.ProductService;

namespace ComercioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            return product;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            bool result = await _productService.UpdateProductAsync(id, product);
            if (result)
            {
                return Ok(new { message = "Producto actualizado" });
            }
            else
            {
                return BadRequest(new { message = "Error al actualizar producto" });
            }

        }

        // POST: api/Product
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Product>> PostProduct(Product productCreate)
        {
            Product product= await _productService.CreateProductAsync(productCreate);

            return CreatedAtAction("GetProduct", new { id = product.id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool result=await _productService.DeleteProductAsync(id);
                if (result)
                {
                    return Ok(new { message = "Producto eliminado" });
                }
                else
                {
                    return NotFound(new { message = "Error al eliminar Producto" });
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el producto: {ex.Message}");
            }
        }


    }
        
}
