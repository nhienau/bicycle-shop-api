namespace api.Utilities
{
    public class PaginatedResponse<T>
    {
        public List<T> Content { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalElements { get; set; }
        //public decimal TotalPrice { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse(List<T> Content, int PageNumber, int PageSize, int TotalElements)
        {
            this.Content = Content;
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.TotalElements = TotalElements;
            TotalPages = (int) Math.Ceiling((double) TotalElements / PageSize);
            // Tính tổng price nếu item có thuộc tính Price
            //foreach (var item in Content)
            //{
            //    var productDetailProperty = item.GetType().GetProperty("productDetail");
            //    var priceProperty = productDetailProperty.GetType().GetProperty("Price");
            //    if (priceProperty != null && priceProperty.PropertyType == typeof(decimal))
            //    {
            //        decimal price = (decimal)priceProperty.GetValue(item);
            //        TotalPrice += price;
            //    }
            //}
        }
    }
}
