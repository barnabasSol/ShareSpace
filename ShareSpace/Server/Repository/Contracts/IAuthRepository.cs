using ShareSpace.Server.Entities;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface IAuthRepository
{
    Task<AuthResponse> CreateUser(CreateUserDTO user);
    Task<AuthResponse> LoginUser(UserLoginDTO login);
    string GenerateAccessToken(User authorized_user, Role role);
    Task<string> GenerateRefershToken(Guid authorized_user_id);
    void UpdateToken();
}
