using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts;

public interface INotificationService
{
    Task<ApiResponse<IEnumerable<NotificationsDto>>> GetNotifications();
    Task<ApiResponse<int>> GetNotificationsCount();
}
