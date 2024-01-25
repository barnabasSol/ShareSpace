using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface ISettingsRepository
{
    Task<ApiResponse<string>> UpdatePassword(UpdatePasswordDto updatePasswordDto, Guid user_id);
    Task<ApiResponse<AuthResponse>> UpdateProfile(UpdateUserProfileDto update_dto, Guid user_id);
}
