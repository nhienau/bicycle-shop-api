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

        //public Cart GetCartByUserId(int userId)
        //{
        //    return _context.Carts
        //        .Include(c => c.Items)
        //        .FirstOrDefault(c => c.UserId == userId);
        //}

        public void UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public async Task SyncCartAsync(int userId, List<CartItemDTO> clientCart)
        {
            // Lấy giỏ hàng hiện tại từ DB
            var dbCart = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();

            foreach (var clientItem in clientCart)
            {
                var existingCartItem = dbCart.FirstOrDefault(c => c.ProductDetailId == clientItem.ProductDetailId);

                if (existingCartItem != null)
                {
                    // Nếu sản phẩm đã tồn tại, cộng dồn số lượng
                    existingCartItem.Quantity += clientItem.quantity;
                }
                else
                {
                    // Nếu chưa tồn tại, thêm mới vào DB
                    _context.Carts.Add(new Cart
                    {
                        ProductDetailId = clientItem.ProductDetailId,
                        //ProductId = clientItem.productId,
                        UserId = userId,
                        Quantity = clientItem.quantity
                    });
                }
            }

            // Lưu thay đổi vào DB
            await _context.SaveChangesAsync();
        }

        public async Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new CartItemDTO
                {
                    productId = (int)c.ProductDetail.ProductId,
                    ProductDetailId = (int)c.ProductDetailId,
                    quantity = c.Quantity,
                    price = c.ProductDetail.Price,
                    name = c.ProductDetail.Product.Name,
                    color = c.ProductDetail.Color,
                    size = c.ProductDetail.Size,
                    //quantityStock = c.ProductDetail.Quantity,
                    image = c.ProductDetail.ProductImage.Url
                }).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddToCartAsync(int userId, CartItemDTO cartItem)
        {
            // Kiểm tra sự tồn tại của ProductDetailId trong bảng ProductDetails
            var productDetailExists = await _context.ProductDetails
                .AnyAsync(pd => pd.Id == cartItem.ProductDetailId);

            if (!productDetailExists)
            {
                throw new ArgumentException("Invalid ProductDetailId.");
            }

            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductDetailId == cartItem.ProductDetailId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.quantity;
            }
            else
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    ProductDetailId = cartItem.ProductDetailId,
                    Quantity = cartItem.quantity
                };
                await _context.Carts.AddAsync(newCart);
            }
        }

        public async Task RemoveFromCartAsync(int userId, int productDetailId)
        {
            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductDetailId == productDetailId);

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
            }
        }
    }
}
