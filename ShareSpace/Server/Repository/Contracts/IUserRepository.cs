using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests();
        Task<ApiResponse<string>> StoreInterests(
            IEnumerable<InterestsDto> interests,
            Guid current_user
        );
        Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId);
        Task<ApiResponse<IEnumerable<SuggestedUserDto>>> GetSuggestedUsers(Guid current_user);
    }
}
