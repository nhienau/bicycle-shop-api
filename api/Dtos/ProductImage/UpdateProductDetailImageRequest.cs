namespace api.Dtos.ProductImage
{
    public class UpdateProductDetailImageRequest
    {
        public int CurrentImageId { get; set; }
        public int NewImageId { get; set; }
        public int ProductDetailId { get; set; }
    }
}
