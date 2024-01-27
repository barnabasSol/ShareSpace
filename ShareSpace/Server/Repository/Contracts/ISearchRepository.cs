using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface ISearchRepository
{
    Task<ApiResponse<IEnumerable<UserSearchDto>>> SearchUser(string value, Guid current_user);
    Task<ApiResponse<IEnumerable<PostSearchDto>>> SearchPost(string value, Guid current_user);
}
