using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services;


public class CommentService : ICommentService
{
    private readonly IHttpClientFactory http_client;

    public CommentService(IHttpClientFactory http)
    {
        http_client = http;
    }

    public async Task<ApiResponse<Guid>> AddComment(CommentAddDto comment)
    {
        var http = http_client.CreateClient("ShareSpaceApi");
        var response = await http.PostAsJsonAsync("/Comment/add", comment);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<Guid>>();
        return result!;
    }

    public async Task<ApiResponse<string>> DeleteComment(Guid post_id)
    {
        var http = http_client.CreateClient("ShareSpaceApi");
        var url = $"/Comment/delete/{post_id}";
        var response = await http.DeleteAsync(url);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
        return result!;
    }
}