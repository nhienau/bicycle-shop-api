namespace api.Dtos.Order
{
    public class OrderQueryDTO
    {
        public string? StatusName { get; set; } = string.Empty;
        public string? CustomerName { get; set; } = string.Empty;
        public string? FromDate { get; set; } = string.Empty;
        public string? ToDate { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
