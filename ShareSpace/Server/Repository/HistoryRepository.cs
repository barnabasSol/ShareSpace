using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository
{
    public class HistoryRepository : IHistoryRepository
    {
        public Task<ApiResponse<int>> GetHistoryOf(Guid user_id)
        {
            throw new NotImplementedException();
        }
    }
}
