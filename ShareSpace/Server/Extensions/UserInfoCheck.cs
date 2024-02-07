using Microsoft.EntityFrameworkCore;
using ShareSpace.Server.Entities;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Server.Extensions;

public static class UserInfoCheck
{
    public static async Task<bool> EmaiOrUsernameExists(
        this CreateUserDTO requesting_user,
        DbSet<User> users
    )
    {
        return await users.AnyAsync(
            _ => _.UserName == requesting_user.UserName || _.Email == requesting_user.Email
        );
    }
}
