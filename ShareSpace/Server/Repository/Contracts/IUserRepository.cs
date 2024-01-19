﻿using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository.Contracts;

public interface IUserRepository
{
    Task<ApiResponse<IEnumerable<InterestsDto>>> GetInterests();
    Task<ApiResponse<string>> StoreInterests(
        IEnumerable<InterestsDto> interests,
        Guid current_user
    );
    Task<ApiResponse<ExtraUserInfoDto>> GetExtraUserInfo(Guid UserId);
    Task<ApiResponse<IEnumerable<SuggestedUserDto>>> GetSuggestedUsers(Guid current_user);
    Task<ApiResponse<string>> UpdateProfilePhoto();
    public Task<ApiResponse<string>> UpdateProfile();
    public Task<ApiResponse<string>> FollowUser(Guid followed_id, Guid follower_id);
    public Task<ApiResponse<IEnumerable<FollowerUserDto>>> GetFollowers(Guid current_user);
    public Task<ApiResponse<IEnumerable<FollowerUserDto>>> GetFollowing(Guid current_user);
}
