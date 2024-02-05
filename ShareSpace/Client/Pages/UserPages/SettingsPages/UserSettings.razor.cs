using FluentValidation;
using MudBlazor;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages.SettingsPages;

public partial class UserSettings
{
    private readonly UpdateUserForm UpdateUserModel = new();
    private readonly UpdateUserFormValidator Validations = new();
    UpdateUserProfileDto UpdateUserProfileDto = new();
    private bool processing = false;
    private string ProfilePic = "";
    MudForm? form;

    private async void Submit()
    {
        await form!.Validate();
        if (form.IsValid)
        {
            var state = await authstate.GetAuthenticationStateAsync();
            string username = state.User.Claims
                .Where(_ => _.Type == "UserName")
                .Select(_ => _.Value)
                .FirstOrDefault()!;
            string name = state.User.Claims
                .Where(_ => _.Type == "Name")
                .Select(_ => _.Value)
                .FirstOrDefault()!;
            string email = state.User.Claims
                .Where(_ => _.Type == "Email")
                .Select(_ => _.Value)
                .FirstOrDefault()!;
            if (string.IsNullOrEmpty(UpdateUserModel.Username))
            {
                UpdateUserModel.Username = username;
            }
            if (string.IsNullOrEmpty(UpdateUserModel.FullName))
            {
                UpdateUserModel.FullName = name;
            }
            if (string.IsNullOrEmpty(UpdateUserModel.Email))
            {
                UpdateUserModel.Email = email;
            }
            processing = true;
            UpdateUserProfileDto.UserName = UpdateUserModel.Username.Trim();
            UpdateUserProfileDto.Name = UpdateUserModel.FullName.Trim();
            UpdateUserProfileDto.Email = UpdateUserModel.Email.Trim();
            UpdateUserProfileDto.Bio = UpdateUserModel.Bio.Trim();
            UpdateUserProfileDto.OldProfilePicUrl = extraUserInfo!.ProfilePicUrl;

            var result = await SettingsService.UpdateProfile(UpdateUserProfileDto);
            if (result.IsSuccess)
            {
                await localstorage.SetItemAsync("ShareSpaceAccessToken", result.Data!.AccessToken);
                await localstorage.SetItemAsync("ShareSpaceRefreshToken", result.Data.RefreshToken);
                await authstate.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/main");
            }
            else
            {
                processing = false;
                ShowSnackBarWithOptions(message: result.Message, variant: Variant.Filled);
            }
        }
        StateHasChanged();
    }

    private ExtraUserInfoDto? extraUserInfo;

    protected override async Task OnInitializedAsync()
    {
        var state = await authstate.GetAuthenticationStateAsync();
        string current_user_string = state.User.Claims
            .Where(_ => _.Type == "Sub")
            .Select(_ => _.Value)
            .FirstOrDefault()!;

        var result = await UserService.GetExtraUserInfo(Guid.Parse(current_user_string));
        if (result.IsSuccess)
        {
            ProfilePic = result.Data!.ProfilePicUrl!;
            extraUserInfo = result.Data!;
            UpdateUserModel.Bio = extraUserInfo!.Bio ?? string.Empty;
        }
    }

    private class UpdateUserForm
    {
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }

    void ShowSnackBarWithOptions(string message, Variant variant)
    {
        SnackBar.Configuration.SnackbarVariant = variant;
        SnackBar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        SnackBar.Add($"Error: {message}", MudBlazor.Severity.Error);
    }

    private class UpdateUserFormValidator : AbstractValidator<UpdateUserForm>
    {
        public UpdateUserFormValidator()
        {
            RuleFor(x => x.Username)
                .MaximumLength(20)
                .WithMessage("Username length can't be more than 20.");

            RuleFor(x => x.FullName)
                .MaximumLength(20)
                .WithMessage("Full Name length can't be more than 20.");

            RuleFor(x => x.Bio).MaximumLength(150).WithMessage("Bio length can't be more than 50.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email is not valid.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
            async (model, propertyName) =>
            {
                var result = await ValidateAsync(
                    ValidationContext<UpdateUserForm>.CreateWithOptions(
                        (UpdateUserForm)model,
                        x => x.IncludeProperties(propertyName)
                    )
                );
                if (result.IsValid)
                    return Array.Empty<string>();
                return result.Errors.Select(e => e.ErrorMessage);
            };
    }
}
