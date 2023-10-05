using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<DataResponse<IEnumerable<InterestsDto>>> GetInterests();
    }
}
