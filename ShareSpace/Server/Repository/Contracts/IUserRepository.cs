using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<DataResponse<IEnumerable<InterestsDto>>> GetInterests();
        Task<DataResponse<string>> StoreInterests(IEnumerable<InterestsDto> interests, Guid current_user);
        Task<DataResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId);
        Task<DataResponse<IEnumerable<string>>> UploadImages(IEnumerable<string> image_url);
    }
}
