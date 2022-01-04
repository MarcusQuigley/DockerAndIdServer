
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using NewClient.Extensions;

namespace NewClient.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _accessToken;
        public WeatherService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<string> WeatherData()
        {
            var response = await _client.GetAsync("api/weatherforecast");
            var answer = await response.Content.ReadAsStringAsync();
            return answer;
        }
    }

    public interface IWeatherService
    {
        Task<string> WeatherData();
    }
}