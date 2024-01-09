using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IHistoryRepository
    {
        public Task<ApiResponse<int>> GetHistoryOf(Guid user_id);
    }
}
