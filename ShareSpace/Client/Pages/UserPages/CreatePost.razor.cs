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
                int maxFileSize = 1024 * 1024 * 1024;
                using var stream = input_file.OpenReadStream(maxAllowedSize: maxFileSize);
                var buffer = new byte[1024];
                int bytesRead;
                using var memoryStream = new MemoryStream();
                while ((bytesRead = await stream.ReadAsync(buffer)) != 0)
                {
                    await memoryStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                }
                collected_files.Add(
                    new ShareSpace.Shared.DTOs.File()
                    {
                        Name = input_file.Name,
                        Size = input_file.Size,
                        Type = input_file.ContentType,
                        ImageBytes = memoryStream.ToArray()
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
                processing = false;
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
