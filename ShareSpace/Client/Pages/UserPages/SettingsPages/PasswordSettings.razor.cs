using FluentValidation;
using MudBlazor;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Pages.UserPages.SettingsPages;

public partial class PasswordSettings
{
    private readonly PasswordModel passwordModel = new();
    readonly PasswordValidator Validations = new();
    private ApiResponse<string>? response;
    private bool processing = false;
    MudForm? form;

    private async void Submit()
    {
        await form!.Validate();
        if (form.IsValid)
        {
            processing = true;
            response = await SettingsService.UpdatePassword(
                new UpdatePasswordDto
                {
                    OldPassword = passwordModel.OldPassword,
                    NewPassword = passwordModel.NewPassword
                }
            );
            processing = false;
            if (response.IsSuccess)
            {
                NavManager.NavigateTo("/main");
            }
            else
                ShowSnackBarWithOptions(response.Message, Variant.Filled);
        }
        StateHasChanged();
    }

    void ShowSnackBarWithOptions(string message, Variant variant)
    {
        SnackBar.Configuration.SnackbarVariant = variant;
        SnackBar.Configuration.ClearAfterNavigation = true;
        SnackBar.Configuration.VisibleStateDuration = 100;
        SnackBar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        SnackBar.Add($"{message}", MudBlazor.Severity.Error);
    }

    bool isShowOP;
    bool isShowNP;

    InputType OldPasswordInput = InputType.Password;
    InputType NewPasswordInput = InputType.Password;

    string PasswordInputIconOP = Icons.Material.Filled.VisibilityOff;
    string PasswordInputIconNP = Icons.Material.Filled.VisibilityOff;

    void OldPasswordButtonAddornmentChange()
    {
        if (isShowOP)
        {
            isShowOP = false;
            PasswordInputIconOP = Icons.Material.Filled.VisibilityOff;
            OldPasswordInput = InputType.Password;
        }
        else
        {
            isShowOP = true;
            PasswordInputIconOP = Icons.Material.Filled.Visibility;
            OldPasswordInput = InputType.Text;
        }
    }

    void NewPasswordButtonAddornmentChange()
    {
        if (isShowNP)
        {
            isShowNP = false;
            PasswordInputIconNP = Icons.Material.Filled.VisibilityOff;
            NewPasswordInput = InputType.Password;
        }
        else
        {
            isShowNP = true;
            PasswordInputIconNP = Icons.Material.Filled.Visibility;
            NewPasswordInput = InputType.Text;
        }
    }
}

public class PasswordModel
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class PasswordValidator : AbstractValidator<PasswordModel>
{
    public PasswordValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithMessage("Old Password is required.")
            .Length(8, 20)
            .WithMessage("Password must be at least 10 characters long.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New Password is required.")
            .Length(8, 20)
            .WithMessage("Password must be at least 10 characters long.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue =>
        async (model, propertyName) =>
        {
            var result = await ValidateAsync(
                ValidationContext<PasswordModel>.CreateWithOptions(
                    (PasswordModel)model,
                    x => x.IncludeProperties(propertyName)
                )
            );
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
}
