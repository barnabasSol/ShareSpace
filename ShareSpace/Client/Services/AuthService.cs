using System.Net.Http.Json;
using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;

        public AuthService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<AuthResponse> CreateUser(CreateUserDTO userDTO)
        {
            try
            {
                var response = await http.PostAsJsonAsync("Auth/create-user", userDTO);
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (response is not null)
                    return result!;
                throw new Exception($"{response!.StatusCode}");
            }
            catch (Exception ex)
            {
                return new AuthResponse() { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<AuthResponse> LoginUser(UserLoginDTO userDTO)
        {
            try
            {
                var response =
                    await http.PostAsJsonAsync("Auth/login-user", userDTO)
                    ?? throw new Exception("Response was null");

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"either username or password is incorrect");

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                return result ?? throw new Exception("Result was null");
            }
            catch (Exception ex)
            {
                return new AuthResponse() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
