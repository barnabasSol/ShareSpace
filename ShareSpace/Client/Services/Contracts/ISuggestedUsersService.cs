using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface ISuggestedUsersService
    {
        Task<ApiResponse<IEnumerable<SuggestedUserDto>>> GetSuggestedUsers(Guid current_user);
    }
}
