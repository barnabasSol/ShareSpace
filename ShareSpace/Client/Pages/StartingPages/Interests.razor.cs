using MudBlazor;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;

namespace ShareSpace.Client.Pages.StartingPages
{
    public partial class Interests
    {
        bool processing = false;
        public MudChip[]? selected;
        public ApiResponse<string>? SendInterestsResponse;
        ApiResponse<IEnumerable<InterestsDto>> response = new();

        protected override async Task OnInitializedAsync()
        {
            response = await UserService.GetInterests();
        }

        private async void Submit()
        {
            try
            {
                processing = true;
                SendInterestsResponse = await UserService.SendInterests(
                    selected!.ToInterestsIEnumerable()
                );
                if (SendInterestsResponse.IsSuccess)
                {
                    processing = false;
                    NavigationManager.NavigateTo("/main");
                }
                else
                {
                    processing = false;
                }
                this.StateHasChanged();
            }
            catch (Exception ex)
            {
                SendInterestsResponse = new ApiResponse<string>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }

    public static class InterestsExtensions
    {
        public static IEnumerable<InterestsDto> ToInterestsIEnumerable(this MudChip[] interests)
        {
            List<InterestsDto> list_interests = new();
            foreach (var interest in interests)
            {
                list_interests.Add(
                    new InterestsDto
                    {
                        Id = int.Parse(interest.Value.ToString()!),
                        Value = interest.Text
                    }
                );
            }
            return list_interests;
        }
    }
}
