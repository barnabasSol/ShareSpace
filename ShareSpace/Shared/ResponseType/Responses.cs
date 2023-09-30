namespace ShareSpace.Shared.ResponseTypes
{
    public class CreateUserResponse
    {
        public bool IsCreated { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Authorized { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
