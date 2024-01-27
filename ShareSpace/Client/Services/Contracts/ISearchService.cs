using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts;

public interface ISearchService
{
    Task<ApiResponse<List<UserSearchDto>>> SearchUsers(string query_string);
    Task<ApiResponse<List<PostSearchDto>>> SearchPosts(string query_string);
}
