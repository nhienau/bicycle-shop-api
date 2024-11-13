using api.Controllers;
using api.Data;
using api.Dtos.Cart;
using api.Dtos.User;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utilities;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _context;
        public CartRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        //public async Task<List<CartDTO>> GetAllAsync()
        //{
        //    // Lấy tất cả các Cart từ cơ sở dữ liệu và bao gồm ProductDetails
        //    var carts = await _context.Carts
        //        .Include(c => c.ProductDetail) // Bao gồm ProductDetail trong mỗi Cart
        //        .ToListAsync();

        //    // Chuyển đổi từng Cart thành CartDTO
        //    var result = carts.Select(c => c.ToCartDto()).ToList();

        //    return result;
        //}

        //public async Task<List<CartDTO>> GetAllCartsAsync(int? userId, string anonymousId)
        //{
        //    IQueryable<Cart> query = _context.Carts.Include(c => c.ProductDetail);

        //    // Lọc theo userId nếu người dùng đã đăng nhập
        //    if (userId.HasValue)
        //    {
        //        query = query.Where(c => c.UserId == userId.Value);
        //    }
        //    // Nếu người dùng chưa đăng nhập, lọc theo anonymousId
        //    else if (!string.IsNullOrEmpty(anonymousId))
        //    {
        //        query = query.Where(c => c.AnonymousId == anonymousId);
        //    }

        //    // Lấy danh sách Cart và chuyển đổi sang CartDTO
        //    var carts = await query.ToListAsync();
        //    var cartDtos = carts.Select(c => new CartDTO
        //    {
        //        UserId = c.UserId,
        //        AnonymousId = c.AnonymousId,
        //        ProductDetailId = (int)c.ProductDetailId,
        //        Quantity = c.Quantity,
        //        ProductDetail = new ProductDetailDto
        //        {
        //            Id = c.ProductDetail.Id,
        //            Size = c.ProductDetail.Size,
        //            Color = c.ProductDetail.Color,
        //            Price = c.ProductDetail.Price,
        //            Quantity = c.ProductDetail.Quantity,
        //            Status = c.ProductDetail.Status
        //        }
        //    }).ToList();

        //    return cartDtos;
        //}
        public async Task<PaginatedResponse<CartDTO>> GetAllAsync(CartQueryDTO query)
        {
            //IQueryable<Cart> carts= _context.Carts.Include(c => c.ProductDetail).ThenInclude(p => p.Carts).AsQueryable();

            //if (!string.IsNullOrWhiteSpace(query.Name))
            //{
            //    products = products.Where(p => p.Name.Contains(query.Name));
            //}
            IQueryable<Cart> carts = _context.Carts.Include(c => c.ProductDetail).ThenInclude(pd => pd.Product).AsQueryable();

            int totalElements = await carts.CountAsync();

            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await carts.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(c => c.ToCartDto()).ToList();

            return new PaginatedResponse<CartDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
        }

        public async Task<PaginatedResponse<CartDTO>> GetCartByUserIdAsync(int userId, CartQueryDTO query)
        {
            //return _context.Carts.FirstOrDefaultAsync(i => i.UserId == id);
            IQueryable<Cart> carts = _context.Carts.Where(c => c.UserId == userId).Include(c => c.ProductDetail).ThenInclude(pd => pd.Product).AsQueryable();
            int totalElements = await carts.CountAsync();
            int recordsSkipped = (query.PageNumber - 1) * query.PageSize;

            var result = await carts.Skip(recordsSkipped).Take(query.PageSize).ToListAsync();
            var resultDto = result.Select(c => c.ToCartDto()).ToList();

            return new PaginatedResponse<CartDTO>(resultDto, query.PageNumber, query.PageSize, totalElements);
            //return _context.Carts.Include(c => c.ProductDetail).ThenInclude(pd => pd.Product).FirstOrDefaultAsync(i => i.UserId == userId);
        }

    }
}
