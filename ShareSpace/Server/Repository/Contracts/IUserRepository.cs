using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<CreateUserResponse> CreateUser(CreateUserDTO user);
        Task<LoginResponse> LoginUser(UserLoginDTO login);
    }
}
