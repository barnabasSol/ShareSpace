using System.Net.Http.Json;
using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory http_client;

    public AuthService(IHttpClientFactory http)
    {
        http_client = http;
    }

    public async Task<AuthResponse> CreateUser(CreateUserDTO userDTO)
    {
        try
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.PostAsJsonAsync("Auth/create-user", userDTO);
            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (response is not null)
                return result!;
            throw new Exception($"{result!.Message}");
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
            var http = http_client.CreateClient("ShareSpaceApi");
            var response =
                await http.PostAsJsonAsync("Auth/login-user", userDTO)
                ?? throw new Exception("Response was null");

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (!response.IsSuccessStatusCode)
                return result!;

            return result ?? throw new Exception("Result was null");
        }
        catch (Exception ex)
        {
            return new AuthResponse() { IsSuccess = false, Message = ex.Message };
        }
    }
}
