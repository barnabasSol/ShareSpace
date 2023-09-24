using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace ShareSpace.Client.Pages.LandingPages
{
    public partial class RequiredUserData
    {

        private readonly RegisterAccountForm ReigisterModel = new();

        private void OnValidSubmit(EditContext context)
        {
            StateHasChanged();
        }

        private class RegisterAccountForm
        {
            [Required]
            [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
            public string Username { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(30, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8), RegularExpression("skdlhbf", ErrorMessage = "must only be letters or numbers or both")]
            public string Password { get; set; } = string.Empty;

            [Required]
            [Compare(nameof(Password))]
            public string Password2 { get; set; } = string.Empty;
        }
    }

   
}
