namespace api.Utilities
{
    public class PaginatedResponse<T>
    {
        public List<T> Content { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResponse(List<T> Content, int PageNumber, int PageSize, int TotalElements)
        {
            this.Content = Content;
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.TotalElements = TotalElements;
            TotalPages = (int) Math.Ceiling((double) TotalElements / PageSize);
        }
    }
}
