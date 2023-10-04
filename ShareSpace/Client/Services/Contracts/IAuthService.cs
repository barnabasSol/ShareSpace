using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> CreateUser(CreateUserDTO userDTO);
        Task<AuthResponse> LoginUser(UserLoginDTO userDTO);
    }
}
