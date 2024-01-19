using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests();
        Task<ApiResponse<string>> SendInterests(IEnumerable<InterestsDto> interests);
        Task<ApiResponse<string>> FollowUser(Guid user_id);
        Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid user_id);
        Task<ApiResponse<List<SuggestedUserDto>>> GetSuggestedUsers();
        Task<ApiResponse<List<FollowerUserDto>>> GetFollowers();
        Task<ApiResponse<List<FollowerUserDto>>> GetFollowing();
    }
}
