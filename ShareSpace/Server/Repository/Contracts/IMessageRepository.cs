using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IMessageRepository
    {
        Task<ApiResponse<IEnumerable<MessageDto>>> GetMessagesOfUser(Guid user_id);
        Task<ApiResponse<IEnumerable<UserMessageDto>>> GetUsersInChat(string username);
        Task<ApiResponse<string>> StoreMessage(MessageDto message);
        Task<ApiResponse<string>> DeleteMessage(Guid message_id);
    }
}
