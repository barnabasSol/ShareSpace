using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ShareSpaceDbContext shareSpaceDb;
        private readonly INotificationRepostiory notificationRepostiory;

        public UserRepository(
            ShareSpaceDbContext shareSpaceDb,
            INotificationRepostiory notificationRepostiory
        )
        {
            this.shareSpaceDb = shareSpaceDb;
            this.notificationRepostiory = notificationRepostiory;
        }

        public async Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId)
        {
            try
            {
                var user_info = await shareSpaceDb.Users
                    .Where(w => w.UserId.Equals(UserId))
                    .Select(
                        x =>
                            new ExtraUserInfoDto
                            {
                                ProfilePicUrl = x.ProfilePicUrl,
                                FollowersCount = shareSpaceDb.Followers
                                    .Where(w => w.FollowedId.Equals(UserId))
                                    .Count(),
                                FollowingCount = shareSpaceDb.Followers
                                    .Where(w => w.FollowerId.Equals(UserId))
                                    .Count(),
                                JoinedDate = x.CreatedAt,
                                Interests = shareSpaceDb.UserInterests
                                    .Where(w => w.UserId.Equals(UserId))
                                    .Join(
                                        shareSpaceDb.Interests,
                                        user_int => user_int.InterestId,
                                        interest => interest.Id,
                                        (uintr, intr) =>
                                            new InterestsDto
                                            {
                                                Id = uintr.InterestId,
                                                Value = intr.InterestName
                                            }
                                    )
                                    .ToList(),
                                Bio = x.Bio
                            }
                    )
                    .FirstAsync();
                return new ApiResponse<ExtraUserInfoDto>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = user_info
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests()
        {
            try
            {
                var interests = await shareSpaceDb.Interests.ToListAsync();
                return new ApiResponse<IEnumerable<InterestsDto>>
                {
                    IsSuccess = true,
                    Data = interests.Select(
                        s => new InterestsDto { Id = s.Id, Value = s.InterestName }
                    )
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> StoreInterests(
            IEnumerable<InterestsDto> interests,
            Guid current_user
        )
        {
            using var transaction = shareSpaceDb.Database.BeginTransaction();
            try
            {
                if (interests.Any())
                {
                    foreach (var interest in interests)
                    {
                        await shareSpaceDb.UserInterests.AddAsync(
                            new UserInterest { InterestId = interest.Id, UserId = current_user }
                        );
                    }
                    await shareSpaceDb.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new ApiResponse<string> { IsSuccess = true };
                }
                throw new Exception($"the received content is not valid");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<SuggestedUserDto>>> GetSuggestedUsers(
            Guid current_user
        )
        {
            try
            {
                var suggested_users = await shareSpaceDb.Users
                    .Where(
                        w =>
                            w.UserId != current_user
                            && !shareSpaceDb.Followers.Any(
                                a => a.FollowerId == current_user && a.FollowedId == w.UserId
                            )
                    )
                    .ToListAsync();
                return new ApiResponse<IEnumerable<SuggestedUserDto>>
                {
                    IsSuccess = true,
                    Message = "",
                    Data = suggested_users.Select(
                        s =>
                            new SuggestedUserDto
                            {
                                Name = s.Name,
                                ProfilePicUrl = s.ProfilePicUrl,
                                UserName = s.UserName,
                                UserId = s.UserId
                            }
                    )
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> FollowUser(Guid followed_id, Guid follower_id)
        {
            var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
            try
            {
                var user = await shareSpaceDb.Followers.FirstOrDefaultAsync(
                    f => f.FollowedId == followed_id && f.FollowerId == follower_id
                );

                if (user is null)
                {
                    await shareSpaceDb.Followers.AddAsync(
                        new Follower { FollowedId = followed_id, FollowerId = follower_id }
                    );
                    notificationRepostiory.AddNotification(
                        source_id: follower_id,
                        user_id: followed_id,
                        status: Status.Followed
                    );
                }
                else
                {
                    notificationRepostiory.AddNotification(
                        source_id: follower_id,
                        user_id: followed_id,
                        status: Status.Unfollowed
                    );
                    shareSpaceDb.Followers.Remove(user);
                }
                await transaction.CommitAsync();
                await shareSpaceDb.SaveChangesAsync();
                return new ApiResponse<string> { IsSuccess = true, };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"operation failed, try again {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<FollowerUserDto>>> GetFollowers(Guid current_user)
        {
            try
            {
                var followerIds = await shareSpaceDb.Followers
                    .Where(f => f.FollowedId == current_user)
                    .Select(f => f.FollowerId)
                    .ToListAsync();

                var followerUsers = await shareSpaceDb.Users
                    .Where(u => followerIds.Contains(u.UserId))
                    .ToListAsync();
                if (!followerUsers.Any())
                {
                    return new ApiResponse<IEnumerable<FollowerUserDto>>
                    {
                        IsSuccess = true,
                        Data = Enumerable.Empty<FollowerUserDto>(),
                        Message = "you don't have any followers"
                    };
                }
                return new ApiResponse<IEnumerable<FollowerUserDto>>
                {
                    IsSuccess = true,
                    Data = followerUsers.Select(
                        s =>
                            new FollowerUserDto
                            {
                                UserId = s.UserId,
                                UserName = s.UserName,
                                Name = s.Name,
                                ProfilePicUrl = s.ProfilePicUrl,
                                IsBeingFollowed = shareSpaceDb.Followers.Any(
                                    a => a.FollowedId == s.UserId && a.FollowerId == current_user
                                )
                            }
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<FollowerUserDto>>> GetFollowing(Guid current_user)
        {
            try
            {
                var followingIds = await shareSpaceDb.Followers
                    .Where(f => f.FollowerId == current_user)
                    .Select(f => f.FollowedId)
                    .ToListAsync();

                var followingUsers = await shareSpaceDb.Users
                    .Where(u => followingIds.Contains(u.UserId))
                    .ToListAsync();
                if (!followingUsers.Any())
                {
                    return new ApiResponse<IEnumerable<FollowerUserDto>>
                    {
                        IsSuccess = true,
                        Data = Enumerable.Empty<FollowerUserDto>(),
                        Message = "you don't have any followers"
                    };
                }
                return new ApiResponse<IEnumerable<FollowerUserDto>>
                {
                    IsSuccess = true,
                    Data = followingUsers.Select(
                        s =>
                            new FollowerUserDto
                            {
                                UserId = s.UserId,
                                UserName = s.UserName,
                                Name = s.Name,
                                ProfilePicUrl = s.ProfilePicUrl,
                                IsBeingFollowed = true
                            }
                    ),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
