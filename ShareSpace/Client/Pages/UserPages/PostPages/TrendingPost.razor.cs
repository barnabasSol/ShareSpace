using ShareSpace.Shared.DTOs;

namespace ShareSpace.Client.Pages.UserPages.PostPages;

public partial class TrendingPost
{
    List<PostDto>? trendingPosts;

    protected override async Task OnInitializedAsync()
    {
        var result = await PostService.GetTrendingPosts();
        if (result.IsSuccess)
        {
            trendingPosts = result.Data!;
        }
    }
    
    void OpenPost(Guid postid){
        NavManager.NavigateTo($"/postdetails/{postid}");

    }
}
