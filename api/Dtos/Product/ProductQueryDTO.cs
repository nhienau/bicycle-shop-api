namespace api.Dtos.Product
{
    public class ProductQueryDTO
    {
        public String Name { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
