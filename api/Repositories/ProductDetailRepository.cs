using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductDetailRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<ProductDetail?> GetByIdAsync(int id)
        {
            return await _context.ProductDetails.FindAsync(id);
        }
    }
}
