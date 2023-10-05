using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
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
            var interests = await shareSpaceDb.Interests.ToListAsync();
            return new DataResponse<IEnumerable<InterestsDto>>()
            {
                IsSuccess = true,
                Data = interests.Select(
                    s => new InterestsDto() { Id = s.Id, Value = s.InterestName }
                )
            };
        }
    }
}
