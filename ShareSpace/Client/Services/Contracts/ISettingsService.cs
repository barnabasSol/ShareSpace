using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts;

public interface ISettingsService
{
    Task<ApiResponse<AuthResponse>> UpdateProfile(UpdateUserProfileDto profileDto);
}
