using Blazored.LocalStorage;

namespace ShareSpace.Client
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;
        public CustomHttpHandler(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            if (request.RequestUri!.AbsolutePath.ToLower().Contains("Auth"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var token = await localStorage.GetItemAsync<string>("ShareSpaceAccessToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
