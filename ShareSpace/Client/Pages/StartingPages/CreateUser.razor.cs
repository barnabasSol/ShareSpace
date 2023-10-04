using ShareSpace.Shared.DTOs;
using FluentValidation;
using MudBlazor;

namespace ShareSpace.Client.Pages.StartingPages
{
    public partial class CreateUser
    {
        private readonly RegisterAccountForm ReigisterModel = new();
        private readonly RegisterAccountFormValidator Validations = new();
        private string message = "Enter The Required Fields Properly.";
        MudForm? form;
        

        private async void Submit()
        {
            await form!.Validate();
            if (form.IsValid)
            {
                var result = await UserService.CreateUser(
                    new CreateUserDTO()
                    {
                        UserName = ReigisterModel.Username,
                        Name = ReigisterModel.FullName,
                        Email = ReigisterModel.Email,
                        Password = ReigisterModel.Password
                    }
                );
                if (result.IsSuccess){
                    NavigationManager.NavigateTo("/interests");
                    await localstorage.SetItemAsync("ShareSpaceToken", result.Token);
                    (authstate as CustomAuthenticationStateProvider)!.NotifyAuthStateChange();
                    NavigationManager.NavigateTo("/interests");
                }
                message = result.Message;
            }
            this.StateHasChanged();
        }


        private class RegisterAccountForm
        {
            public string Username { get; set; } = string.Empty;

            public string FullName { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;

            public string Password { get; set; } = string.Empty;

            public string Password2 { get; set; } = string.Empty;
        }

        private class RegisterAccountFormValidator : AbstractValidator<RegisterAccountForm>
        {
            public RegisterAccountFormValidator()
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .WithMessage("Username is required.")
                    .Length(1, 20)
                    .WithMessage("Username length can't be more than 20.");

                RuleFor(x => x.FullName)
                    .NotEmpty()
                    .WithMessage("Full Name is required.")
                    .Length(1, 8)
                    .WithMessage("Full Name length can't be more than 8.");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("Email is required.")
                    .EmailAddress()
                    .WithMessage("Email is not valid.");

                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password is required.")
                    .Length(8, 30)
                    .WithMessage("Password must be at least 8 characters long.");

                RuleFor(x => x.Password2)
                    .NotEmpty()
                    .WithMessage("Confirmation Password is required.")
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match.");
            }

            public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
                async (model, propertyName) =>
                {
                    var result = await ValidateAsync(
                        ValidationContext<RegisterAccountForm>.CreateWithOptions(
                            (RegisterAccountForm)model,
                            x => x.IncludeProperties(propertyName)
                        )
                    );
                    if (result.IsValid)
                        return Array.Empty<string>();
                    return result.Errors.Select(e => e.ErrorMessage);
                };
        }
    }
}
