using System.Net.Http.Json;
using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services
{
    public class MessageServices : IMessageService
    {
        private readonly IHttpClientFactory http_client;

        public MessageServices(IHttpClientFactory http)
        {
            http_client = http;
        }

        public async Task<ApiResponse<IEnumerable<MessageDto>>> GetUserMessages()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/get-messages");
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<IEnumerable<MessageDto>>
            >();
            return result!;
        }

        public async Task<ApiResponse<IEnumerable<UserMessageDto>>> GetUsersInChat()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.GetAsync("/users-chat");
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserMessageDto>>>();
            return result!;
        }

        public async Task<ApiResponse<string>> StoreMessage(MessageDto message)
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var response = await http.PostAsJsonAsync("/store-message", message);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
            return result!;
        }
    }
}
