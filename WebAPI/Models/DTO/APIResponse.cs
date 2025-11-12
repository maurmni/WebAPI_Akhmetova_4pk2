namespace WebAPI.DTOs
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public APIResponse() { }

        public APIResponse(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }

        public APIResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
        }
    }
    public class APIResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public APIResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }
    }
}