using System.Net.Http.Json;
using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory http_client;

        public NotificationService(IHttpClientFactory http)
        {
            http_client = http;
        }

        public async Task<ApiResponse<IEnumerable<NotificationsDto>>> GetNotifications()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/Notification/notifications";
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<
                ApiResponse<IEnumerable<NotificationsDto>>
            >();
            return result!;
        }

        public async Task<ApiResponse<int>> GetNotificationsCount()
        {
            var http = http_client.CreateClient("ShareSpaceApi");
            var url = $"/Notification/count";
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
            return result!;
        }
    }
}
