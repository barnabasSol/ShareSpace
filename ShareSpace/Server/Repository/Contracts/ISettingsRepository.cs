using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface ISettingsRepository
{
    Task<ApiResponse<string>> UpdateProfilePhoto();
    Task<ApiResponse<string>> UpdateProfile();
}
