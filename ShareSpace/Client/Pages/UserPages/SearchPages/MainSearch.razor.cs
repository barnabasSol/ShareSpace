using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages.SearchPages;

public partial class MainSearch
{
    List<SuggestedUserDto>? SuggestedUsers;
    public List<PostDto> Posts { get; set; } = new();
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var response = await UserService.GetSuggestedUsers();
        if (response.IsSuccess)
        {
            SuggestedUsers = response.Data!;
        }
        var request = await PostService.GetPosts();
        if (request.Data is not null)
        {
            Posts = request.Data;
        }
        isLoading = false;
    }

    async void FollowUser(SuggestedUserDto user)
    {
        var result = await UserService.FollowUser(user.UserId);
        if (result.IsSuccess)
        {
            SuggestedUsers!.Remove(user);
        }
        StateHasChanged();
    }

    async void SearchedFollow(UserSearchDto user)
    {
        var result = await UserService.FollowUser(user.UserId);
        if (result.IsSuccess)
        {
            user.IsBeingFollowed = !user.IsBeingFollowed;
        }
        StateHasChanged();
    }

    async void SearchedUnfollow(UserSearchDto user)
    {
        var result = await UserService.FollowUser(user.UserId);
        if (result.IsSuccess)
        {
            user.IsBeingFollowed = !user.IsBeingFollowed;
        }
        StateHasChanged();
    }

    async void DeletePost(PostDto postToDelete)
    {
        var result = await PostService.DeletePost(postToDelete.PostId);
        if (result.IsSuccess)
        {
            Posts.Remove(postToDelete);
            StateHasChanged();
        }
    }

    async void DeleteSearchedPost(PostSearchDto postToDelete)
    {
        var result = await PostService.DeletePost(postToDelete.PostId);
        if (result.IsSuccess)
        {
            postSearchDtos!.Remove(postToDelete);
            StateHasChanged();
        }
    }
}
