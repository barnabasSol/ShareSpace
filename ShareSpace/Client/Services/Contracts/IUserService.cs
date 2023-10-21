using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests();
        Task<ApiResponse<string>> SendInterests(IEnumerable<InterestsDto> interests);
        Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId);
    }
}
