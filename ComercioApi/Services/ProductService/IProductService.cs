using ComercioApi.Models;

namespace ComercioApi.Services.ProductService.ProductService
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<bool> UpdateProductAsync(int id, Product product);
        public Task<Product> CreateProductAsync(Product product);
        public Task<bool> DeleteProductAsync(int id);
    }
}
