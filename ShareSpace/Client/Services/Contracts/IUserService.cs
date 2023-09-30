using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateUser(CreateUserDTO userDTO);
        Task<LoginResponse> LoginUser(UserLoginDTO userDTO);
    }
}
