namespace ShareSpace.Shared.ResponseTypes
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DataResponse<T>
    {
        public bool IsSuccess { get; set;}
        public string Message { get; set;} = string.Empty;
        public T? Data { get; set;}
    }
}