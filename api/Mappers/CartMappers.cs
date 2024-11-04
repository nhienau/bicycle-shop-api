using api.Dtos.Cart;
using api.Models;

namespace api.Mappers
{
    public static class CartMappers
    {
        public static CartDTO ToCartDto(this Cart cartModel)
        {
            return new CartDTO
            {
                UserId = (int)cartModel.UserId,
                ProductDetailId = (int)cartModel.ProductDetailId,
                Quantity = cartModel.Quantity,
                //ProductList = cartModel.ProductList.Select(pd => pd.ToProductDto()).ToList()
                ProductDetail = cartModel.ProductDetail.ToProductDetailDto1()
            };
        }

        //public static CartDTO ToCartDto(this Cart cartModel)
        //{
        //    return new CartDTO
        //    {
        //        UserId = (int)cartModel.UserId,
        //        ProductDetailId = (int)cartModel.ProductDetailId,
        //        Quantity = cartModel.Quantity,
        //        //ProductList = cartModel.ProductList.Select(pd => pd.ToProductDto()).ToList()
        //        //ProductDetail = cartModel.ProductDetail.ToProductDetailDto()
        //    };
        //}


        //public static Product ToProductFromCreateDto(this CreateProductRequestDto productDto)
        //{
        //    return new Product
        //    {
        //        Name = productDto.Name,
        //        Description = productDto.Description,
        //        ProductCategoryId = productDto.ProductCategoryId,
        //        Status = true,
        //    };
        //}
    }
}
