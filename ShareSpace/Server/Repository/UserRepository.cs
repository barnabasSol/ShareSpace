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

        public UserRepository(ShareSpaceDbContext shareSpaceDb)
        {
            this.shareSpaceDb = shareSpaceDb;
        }

        public async Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId)
        {
            try
            {
                var user = await shareSpaceDb.Users
                    .Where(w => w.UserId.Equals(UserId))
                    .Select(
                        x =>
                            new ExtraUserInfoDto()
                            {
                                ProfilePicUrl = x.ProfilePicUrl,
                                FollowersCount = shareSpaceDb.Followers
                                    .Where(w => w.FollowedId.Equals(UserId))
                                    .Count(),
                                FollowingCount = shareSpaceDb.Followers
                                    .Where(w => w.FollowerId.Equals(UserId))
                                    .Count(),
                                JoinedDate = x.CreatedAt,
                                Interests = shareSpaceDb.UserInterests.Where(w => w.UserId.Equals(UserId)).Join(shareSpaceDb.Interests,
                                user_int => user_int.InterestId,
                                interest => interest.Id,
                                (uintr, intr)=> new InterestsDto()
                                {
                                    Id = uintr.InterestId,
                                    Value = intr.InterestName
                                }).ToList(),
                                Bio = x.Bio
                            }
                    )
                    .FirstAsync();
                return new ApiResponse<ExtraUserInfoDto>()
                {
                    IsSuccess = true,
                    Message = "",
                    Data = user
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
                return new ApiResponse<IEnumerable<InterestsDto>>()
                {
                    IsSuccess = true,
                    Data = interests.Select(
                        s => new InterestsDto() { Id = s.Id, Value = s.InterestName }
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
                            new UserInterest() { InterestId = interest.Id, UserId = current_user }
                        );
                    }
                    await shareSpaceDb.SaveChangesAsync();
                    transaction.Commit();
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
    }
}
