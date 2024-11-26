namespace api.Utilities
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public object Error { get; set; }

        public ApiResponse()
        {
            
        }

        // Success response with data
        public static ApiResponse<T> Success(T data,int statusCode = 200, string message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Message = message ?? "Success",
                Result = data,
                Error = null
            };
        }

        // Success response without data
        public static ApiResponse<T> Success(int statusCode = 200, string message = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = true,
                Message = message ?? "Success",
                Result = default,
                Error = null
            };
        }

        // Error response
        public static ApiResponse<T> Fail(int statusCode, string message = null, object errors = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                IsSuccess = false,
                Message = message ?? "Fail",
                Result = default,
                Error = errors
            };
        }
    }
}
