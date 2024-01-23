using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository;

public class SettingsRepository : ISettingsRepository
{
    private readonly ShareSpaceDbContext shareSpaceDb;
    private readonly IAuthRepository authRepository;

    public SettingsRepository(ShareSpaceDbContext shareSpaceDb, IAuthRepository authRepository)
    {
        this.shareSpaceDb = shareSpaceDb;
        this.authRepository = authRepository;
    }
    public Task<ApiResponse<string>> UpdateProfile()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<string>> UpdateProfilePhoto()
    {
        throw new NotImplementedException();
    }
}
