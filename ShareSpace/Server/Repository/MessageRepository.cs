using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ShareSpaceDbContext shareSpaceDb;

        public MessageRepository(ShareSpaceDbContext shareSpaceDb)
        {
            this.shareSpaceDb = shareSpaceDb;
        }

        public Task<ApiResponse<string>> DeleteMessage(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<MessageDto>>> GetMessagesOfUser(Guid user_id)
        {
            try
            {
                var receiver = shareSpaceDb.Users
                    .Where(w => w.UserId == user_id)
                    .Select(s => s.UserName)
                    .FirstOrDefault();

                var messages = await shareSpaceDb.Messages
                    .Where(w => w.SenderId.Equals(user_id) || w.ReceiverId.Equals(user_id))
                    .Select(
                        s =>
                            new MessageDto()
                            {
                                From = s.SenderId,
                                To = receiver!,
                                Text = s.Content
                            }
                    )
                    .ToListAsync();
                if (messages.Count < 1)
                {
                    return new ApiResponse<IEnumerable<MessageDto>>()
                    {
                        IsSuccess = true,
                        Message = "",
                        Data = Enumerable.Empty<MessageDto>()
                    };
                }
                return new ApiResponse<IEnumerable<MessageDto>>()
                {
                    IsSuccess = true,
                    Message = "",
                    Data = messages
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<UserMessageDto>>> GetUsersInChat(string username)
        {
            try
            {
                Guid current_user_guid = shareSpaceDb.Users
                    .FirstOrDefault(w => w.UserName == username)!
                    .UserId;
                var messages = await shareSpaceDb.Messages
                    .Where(
                        m =>
                            m.SenderId.Equals(current_user_guid)
                            || m.ReceiverId.Equals(current_user_guid)
                    )
                    .ToListAsync();

                var users_in_chat = messages
                    .GroupBy(m => m.SenderId.Equals(current_user_guid) ? m.ReceiverId : m.SenderId)
                    .Select(
                        g =>
                            new
                            {
                                UserId = g.Key,
                                LastMessage = g.OrderByDescending(m => m.CreatedAt).FirstOrDefault()
                            }
                    )
                    .Join(
                        shareSpaceDb.Users,
                        chat => chat.UserId,
                        user => user.UserId,
                        (chat, user) =>
                            new UserMessageDto
                            {
                                UserId = user.UserId,
                                UserName = user.UserName,
                                Name = user.Name,
                                ProfilePicUrl = user.ProfilePicUrl,
                                Message = chat.LastMessage!.Content,
                                SentDateTime = chat.LastMessage.CreatedAt
                            }
                    )
                    .OrderByDescending(dto => dto.SentDateTime)
                    .ToList();
                return new ApiResponse<IEnumerable<UserMessageDto>>()
                {
                    IsSuccess = true,
                    Message = "",
                    Data = users_in_chat
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> StoreMessage(MessageDto message)
        {
            try
            {
                Guid receiver = shareSpaceDb.Users
                    .Where(w => w.UserName == message.To)
                    .Select(s => s.UserId)
                    .FirstOrDefault();
                await shareSpaceDb.Messages.AddAsync(
                    new Message()
                    {
                        Content = message.Text,
                        SenderId = message.From,
                        ReceiverId = receiver,
                    }
                );
                await shareSpaceDb.SaveChangesAsync();
                return new ApiResponse<string>()
                {
                    IsSuccess = true,
                    Message = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
