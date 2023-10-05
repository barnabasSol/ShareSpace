using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<DataResponse<IEnumerable<InterestsDto>>> GetInterests();
    }
}
