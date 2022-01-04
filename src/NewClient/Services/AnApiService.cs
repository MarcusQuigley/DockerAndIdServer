using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using NewClient.Extensions;

namespace NewClient.Services
{
    public class AnApiService : IAnApiService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _accessToken;
        public AnApiService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;

        }

        private async Task<string> GetToken()
        {
            if (!string.IsNullOrWhiteSpace(_accessToken))
            {
                return _accessToken;
            }

            var discoveryDocumentResponse =
                await _client.GetDiscoveryDocumentAsync(_configuration["IdentityUri"]);
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var tokenResponse =
                await _client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = discoveryDocumentResponse.TokenEndpoint,
                        ClientId = "client", //"globoticketm2m",
                        ClientSecret = "secret",  //"eac7008f-1b35-4325-ac8d-4a71932e6088",
                        Scope = "api1"
                    });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        public async Task<string> UserData()
        {
            _client.SetBearerToken(await GetToken());
            var response = await _client.GetAsync("api/identity");
            var answer = await response.Content.ReadAsStringAsync();
            return answer;
        }
    }

    public interface IAnApiService
    {
        Task<string> UserData();
    }
}