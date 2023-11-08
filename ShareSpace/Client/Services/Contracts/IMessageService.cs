using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IMessageService
    {
        Task<ApiResponse<string>> StoreMessage(MessageDto message);
        Task<ApiResponse<IEnumerable<MessageDto>>> GetUserMessages();
        Task<ApiResponse<IEnumerable<UserMessageDto>>> GetUsersInChat();
    }
}
