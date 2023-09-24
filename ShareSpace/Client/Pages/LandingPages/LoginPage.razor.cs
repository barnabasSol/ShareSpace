using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.LandingPages
{
    public partial class LoginPage
    {
        private readonly Login LoginModel = new();


        private async void OnValidSubmit(EditContext context)
        {
            StateHasChanged();
            await LoginUser();
        }

        public async Task LoginUser()
        {
            UserDto user = new()
            {
                Id = 1,
                UserName = LoginModel.Username,
                Password = Encoding.UTF8.GetBytes(LoginModel.Password),

            };
            await http.PostAsJsonAsync<UserDto>("/Auth/loginuser", user);
        }
    }

    public class Login
    {
        [Required]
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
