using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IAuthRepository
    {
        Task<AuthResponse> CreateUser(CreateUserDTO user);
        Task<AuthResponse> LoginUser(UserLoginDTO login);
        Task<AuthResponse> UpdateToken();

    }
}
