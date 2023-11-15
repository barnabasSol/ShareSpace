using Microsoft.EntityFrameworkCore;
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

        public async Task<ApiResponse<IEnumerable<MessageDto>>> GetMessagesOfUser(
            Guid current_user,
            string other_user
        )
        {
            try
            {
                var other_user_guid = await shareSpaceDb.Users.FirstAsync(
                    f => f.UserName == other_user
                );

                var messages = await shareSpaceDb.Messages
                    .Where(
                        w =>
                            (
                                w.SenderId.Equals(current_user)
                                && w.ReceiverId.Equals(other_user_guid.UserId)
                            )
                            || (
                                w.SenderId.Equals(other_user_guid.UserId)
                                && w.ReceiverId.Equals(current_user)
                            )
                    )
                    .Select(
                        s =>
                            new MessageDto
                            {
                                MessageId = s.MessageId,
                                To = shareSpaceDb.Users
                                    .FirstOrDefault(f => f.UserId.Equals(other_user_guid))!
                                    .UserName,
                                Text = s.Content,
                                Seen = s.Seen,
                                SentDateTime = s.CreatedAt
                            }
                    )
                    .ToListAsync();
                if (messages.Count < 1)
                {
                    return new ApiResponse<IEnumerable<MessageDto>>
                    {
                        IsSuccess = true,
                        Data = Enumerable.Empty<MessageDto>(),
                        Message = ""
                    };
                }
                return new ApiResponse<IEnumerable<MessageDto>>
                {
                    IsSuccess = true,
                    Data = messages,
                    Message = ""
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
                    .First(w => w.UserName == username)!
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
                    .Select(g => new { UserId = g.Key, LastMessage = g.MaxBy(b => b.CreatedAt) })
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
                    .OrderByDescending(o => o.SentDateTime)
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
