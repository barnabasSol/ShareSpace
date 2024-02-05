using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Data;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using BC = BCrypt.Net.BCrypt;

namespace ShareSpace.Server.Repository;

public class SettingsRepository : ISettingsRepository
{
    private readonly ShareSpaceDbContext shareSpaceDb;
    private readonly IAuthRepository authRepository;
    private readonly IWebHostEnvironment webHost;

    public SettingsRepository(
        ShareSpaceDbContext shareSpaceDb,
        IAuthRepository authRepository,
        IWebHostEnvironment webHost
    )
    {
        this.shareSpaceDb = shareSpaceDb;
        this.authRepository = authRepository;
        this.webHost = webHost;
    }

    public async Task<ApiResponse<string>> UpdatePassword(
        UpdatePasswordDto updatePasswordDto,
        Guid user_id
    )
    {
        var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
        try
        {
            var user = await shareSpaceDb.Users.FindAsync(user_id);
            if (user is null)
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "user doesn't exist"
                };
            }
            if (
                string.IsNullOrEmpty(updatePasswordDto.OldPassword)
                || string.IsNullOrEmpty(updatePasswordDto.NewPassword)
            )
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "proper input isn't given"
                };
            }
            if (!BC.Verify(updatePasswordDto.OldPassword, user.PasswordHash))
            {
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    Message = "old password is incorrect"
                };
            }
            user.PasswordHash = BC.HashPassword(updatePasswordDto.NewPassword);
            await shareSpaceDb.SaveChangesAsync();
            await transaction.CommitAsync();
            return new ApiResponse<string> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<AuthResponse>> UpdateProfile(
        UpdateUserProfileDto update_dto,
        Guid user_id
    )
    {
        var transaction = await shareSpaceDb.Database.BeginTransactionAsync();
        try
        {
            var user = await shareSpaceDb.Users.FindAsync(user_id);
            if (user is null)
            {
                return new ApiResponse<AuthResponse>
                {
                    IsSuccess = false,
                    Message = "User Doesn't Exist"
                };
            }
            if (
                await shareSpaceDb.Users.AnyAsync(a => a.UserName == update_dto.UserName)
                && user.UserName != update_dto.UserName
            )
            {
                return new ApiResponse<AuthResponse>
                {
                    IsSuccess = false,
                    Message = "Username Is In Use"
                };
            }

            if (
                await shareSpaceDb.Users.AnyAsync(a => a.Email == update_dto.Email)
                && user.Email != update_dto.Email
            )
            {
                return new ApiResponse<AuthResponse>
                {
                    IsSuccess = false,
                    Message = "Email Is In Use"
                };
            }

            if (update_dto.ProfilePic is not null)
            {
                string FileExtension = update_dto.ProfilePic.Type.ToLower() switch
                {
                    string type when type.Contains("png") => "png",
                    string type when type.Contains("jpeg") => "jpeg",
                    string type when type.Contains("webp") => "webp",
                    _ => throw new Exception("Invalid file format!")
                };
                string img_url = $"Uploads/ProfilePictures/{Guid.NewGuid()}.{FileExtension}";
                user.ProfilePicUrl = img_url;
                string webRootPath = webHost.WebRootPath;
                string NewFileName = Path.Combine(webRootPath, img_url);
                if (!string.IsNullOrEmpty(update_dto.OldProfilePicUrl))
                {
                    var oldImagePath = Path.Combine(
                        webRootPath,
                        update_dto.OldProfilePicUrl.TrimStart('/')
                    );
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                using var FileStream = System.IO.File.Create(NewFileName);
                await FileStream.WriteAsync(update_dto.ProfilePic.ImageBytes);
            }

            user.Bio = update_dto.Bio;
            user.UserName = update_dto.UserName;
            user.Email = update_dto.Email;
            user.Name = update_dto.Name;

            await shareSpaceDb.SaveChangesAsync();
            await transaction.CommitAsync();
            var user_role = shareSpaceDb.UserRoles
                .Where(w => w.UserId == user_id)
                .Select(s => s.RoleId)
                .FirstOrDefault();
            Role role = user_role == 1 ? Role.User : Role.Admin;
            var new_token = authRepository.GenerateAccessToken(user, role);
            return new ApiResponse<AuthResponse>
            {
                IsSuccess = true,
                Data = new AuthResponse
                {
                    AccessToken = new_token,
                    RefreshToken = await authRepository.GenerateRefershToken(user.UserId)
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message);
        }
    }
}
