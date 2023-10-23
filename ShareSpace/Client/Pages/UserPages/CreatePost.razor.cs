using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages
{
    public partial class CreatePost
    {
        public string PostText { get; set; } = string.Empty;

        private List<ShareSpace.Shared.DTOs.File> collected_files = new();

        private bool processing = false;

        private async Task CollectFiles(InputFileChangeEventArgs e)
        {
            foreach (var input_file in e.GetMultipleFiles())
            {
                var buffer = new byte[input_file.Size];
                await input_file.OpenReadStream().ReadAsync(buffer);
                collected_files.Add(
                    new ShareSpace.Shared.DTOs.File()
                    {
                        Name = input_file.Name,
                        Size = input_file.Size,
                        Type = input_file.ContentType,
                        ImageBytes = buffer
                    }
                );
            }
        }

        private async void UploadPost()
        {
            processing = true;
            var response = await PostService.CreatePost(
                new CreatePostDto() { TextContent = PostText, PostFiles = collected_files }
            );
            if (response.IsSuccess)
            {
                processing = false;
                ShowSnackBarWithOptions("it is posted", Variant.Filled, Severity.Success);
            }
            else
            {
                ShowSnackBarWithOptions(response.Message, Variant.Filled, Severity.Error);
            }
            StateHasChanged();
        }

        void ShowSnackBarWithOptions(string message, Variant variant, Severity severity)
        {
            SnackBar.Configuration.SnackbarVariant = variant;
            SnackBar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            SnackBar.Configuration.VisibleStateDuration = 1500;
            SnackBar.Add($"{message}", severity);
        }
    }
}
