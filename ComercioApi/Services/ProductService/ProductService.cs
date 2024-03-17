
using ComercioApi.Context;
using ComercioApi.Models;
using ComercioApi.Services.ProductService.ProductService;
using Microsoft.EntityFrameworkCore;

namespace ComercioApi.Services.ProductService
{
    public class ProductServices : IProductService
    {
        private readonly AppDbContext _context;

        public ProductServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            var productExisting  = await _context.Products.FindAsync(id);
            if (productExisting == null)
            {
                return false;
            }
            product.id = id;
          
            _context.Entry(productExisting).CurrentValues.SetValues(product);


            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
