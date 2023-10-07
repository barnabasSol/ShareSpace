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

        public async Task<DataResponse<IEnumerable<InterestsDto>>> GetInterests()
        {
            try
            {
                var interests = await shareSpaceDb.Interests.ToListAsync();
                return new DataResponse<IEnumerable<InterestsDto>>()
                {
                    IsSuccess = true,
                    Data = interests.Select(
                        s => new InterestsDto() { Id = s.Id, Value = s.InterestName }
                    )
                };
            }
            catch (Exception ex)
            {
                return new DataResponse<IEnumerable<InterestsDto>>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = Enumerable.Empty<InterestsDto>()
                };
            }
        }

        public async Task<DataResponse<string>> StoreInterests(IEnumerable<InterestsDto> interests, Guid current_user)
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
                    return new DataResponse<string> { IsSuccess = true };
                }
                return new DataResponse<string>
                {
                    IsSuccess = false,
                    Message = $"the received content is empty"
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new DataResponse<string>
                {
                    IsSuccess = false,
                    Message = $"couldn't store your interests, {ex.Message}"
                };
            }
        }
    }
}
