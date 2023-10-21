using Microsoft.AspNetCore.Components.Forms;
using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages
{
    public partial class CreatePost
    {
        public string PostText { get; set; } = string.Empty;

        private CreatePostDto postDto = new();

        private List<PostFile> collected_files = new();

        private bool processing = false;

        private bool AllowedToPost;

        private async Task CollectFiles(InputFileChangeEventArgs e)
        {
            foreach (var input_file in e.GetMultipleFiles())
            {
                var buffer = new byte[input_file.Size];
                await input_file.OpenReadStream().ReadAsync(buffer);
                collected_files.Add(
                    new PostFile()
                    {
                        Name = input_file.Name,
                        Size = input_file.Size,
                        Type = input_file.ContentType,
                        ImageBytes = buffer
                    }
                );
            }
        }

        private void IsAllowed()
        {
            if (string.IsNullOrEmpty(PostText))
            {
                AllowedToPost = false;
            }
            else
            {
                AllowedToPost = true;
            }
        }

        private async void UploadPost()
        {
            await PostService.CreatePost(
                new CreatePostDto() { TextContent = PostText, PostFiles = collected_files }
            );
        }
    }
}
