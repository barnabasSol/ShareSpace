using ShareSpace.Client.Services.Contracts;
using ShareSpace.Shared.DTOs;
using ShareSpace.Shared.ResponseTypes;
using System.Net.Http.Json;

namespace ShareSpace.Client.Services
{
    public class SuggestedUsersService : ISuggestedUsersService
    {
        private readonly IHttpClientFactory httpClient;

        public SuggestedUsersService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

    }
}
