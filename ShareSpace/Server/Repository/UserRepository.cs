using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShareSpace.Server.Data;
using ShareSpace.Server.Entities;
using ShareSpace.Server.JWT;
using ShareSpace.Server.Repository.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ShareSpaceDbContext shareSpaceDb;
        private readonly TokenSettings token_Setting;

        public UserRepository(
            ShareSpaceDbContext shareSpaceDb,
            IOptions<TokenSettings> token_setting
        )
        {
            this.shareSpaceDb = shareSpaceDb;
            token_Setting = token_setting.Value;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserDTO requesting_user)
        {
            try
            {
                if (await shareSpaceDb.Users.AnyAsync(_ => _.UserName == requesting_user.UserName))
                {
                    return new CreateUserResponse()
                    {
                        IsCreated = false,
                        Message = "username is in use"
                    };
                }
                if (await shareSpaceDb.Users.AnyAsync(_ => _.Email == requesting_user.Email))
                {
                    return new CreateUserResponse()
                    {
                        IsCreated = false,
                        Message = "email is in use"
                    };
                }

                await shareSpaceDb.Users.AddAsync(
                    new User()
                    {
                        UserName = requesting_user.UserName,
                        Name = requesting_user.Name,
                        Email = requesting_user.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(requesting_user.Password)
                    }
                );

                await shareSpaceDb.SaveChangesAsync();

                return new CreateUserResponse()
                {
                    IsCreated = true,
                    Message = "user is successfully created"
                };
            }
            catch (Exception)
            {
                return new CreateUserResponse()
                {
                    IsCreated = false,
                    Message = "server error happened, try again later"
                };
            }
        }


        public async Task<LoginResponse> LoginUser(UserLoginDTO user_login)
        {
            try
            {
                if (
                    string.IsNullOrEmpty(user_login.UserName)
                    || string.IsNullOrEmpty(user_login.Password)
                )
                {
                    return new LoginResponse()
                    {
                        Authorized = false,
                        Message = "enter the required field"
                    };
                }

                var queried_user = await shareSpaceDb.Users
                    .Where(_ => _.UserName == user_login.UserName)
                    .FirstOrDefaultAsync();

                if (queried_user is null)
                {
                    return new LoginResponse()
                    {
                        Authorized = false,
                        Message = "user doesn't exist"
                    };
                }

                if (!BCrypt.Net.BCrypt.Verify(user_login.Password, queried_user.PasswordHash))
                {
                    return new LoginResponse()
                    {
                        Authorized = false,
                        Message = "incorrect password or username"
                    };
                }

                return new LoginResponse()
                {
                    Authorized = true,
                    Message = "",
                    Token = GenerateToken(queried_user)
                };
            }
            catch (Exception)
            {
                return new LoginResponse()
                {
                    Authorized = false,
                    Message = "something went wrong, try again later."
                };
            }
        }


        private string GenerateToken(User authorized_user)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(token_Setting.SecretKey));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims =
                new()
                {
                    new Claim("Sub", authorized_user.UserId.ToString()),
                    new Claim("UserName", authorized_user.UserName),
                    new Claim("Name", authorized_user.Name),
                    new Claim("Email", authorized_user.Email)
                };
            JwtSecurityToken securityToken =
                new(
                    issuer: token_Setting.Issuer,
                    audience: token_Setting.Audience,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: credentials,
                    claims: claims
                );
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
