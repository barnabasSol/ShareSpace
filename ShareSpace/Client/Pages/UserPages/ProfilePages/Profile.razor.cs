using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages.ProfilePages;

public partial class Profile
{
    private ProfileDto? ProfileData;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrEmpty(username))
        {
            var result = await UserService.GetProfile(username);
            if (result.IsSuccess)
            {
                ProfileData = result.Data;
            }
        }
    }

    async Task DeletePost(PostDto postToDelete)
    {
        var result = await PostService.DeletePost(postToDelete.PostId);
        if (result.IsSuccess)
        {
            ProfileData!.Posts!.Remove(postToDelete);
            StateHasChanged();
        }
    }
}
