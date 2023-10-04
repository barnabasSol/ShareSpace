using FluentValidation;
using MudBlazor;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Pages.StartingPages
{
    public partial class LoginPage
    {
        private string message = "", show = "none";
        private readonly Login LoginModel = new();
        readonly LoginValidator Validations = new();
        private AuthResponse? response;
        private bool processing = false;
        MudForm? form;

        private async void Submit()
        {
            await form!.Validate();
            if (form.IsValid)
            {
                processing = true;
                response = await UserService.LoginUser(
                    new UserLoginDTO()
                    {
                        UserName = LoginModel.Username,
                        Password = LoginModel.Password
                    }
                );
                if (!response.IsSuccess)
                {
                    processing = false;
                    show = "block";
                    message = response.Message;
                }
                else
                {
                    processing = false;
                    await localstorage.SetItemAsync("ShareSpaceToken", response.Token);
                    (authstate as CustomAuthenticationStateProvider)!.NotifyAuthStateChange();
                    NavigationManager.NavigateTo("/main");
                }
            }
            this.StateHasChanged();
        }
    }

    public class Login
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Length(1, 20)
                .WithMessage("Username length can't be more than 20.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .Length(8, 30)
                .WithMessage("Password must be at least 8 characters long.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<Login>.CreateWithOptions(
                        (Login)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
