using System.Net.Http.Json;
using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services;

public class SearchService : ISearchService
{
    private readonly IHttpClientFactory http_client;

    public SearchService(IHttpClientFactory http)
    {
        http_client = http;
    }

    public async Task<ApiResponse<List<PostSearchDto>>> SearchPosts(string query_string)
    {
        var http = http_client.CreateClient("ShareSpaceApi");
        var url = $"/Search/posts/{query_string}";
        var response = await http.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<PostSearchDto>>>();
        return result!;
    }

    public async Task<ApiResponse<List<UserSearchDto>>> SearchUsers(string query_string)
    {
        var http = http_client.CreateClient("ShareSpaceApi");
        var url = $"/Search/users/{query_string}";
        var response = await http.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<UserSearchDto>>>();
        return result!;
    }
}
