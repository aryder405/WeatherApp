
using AlertLogic.WeatherService.models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace AlertLogic.WeatherService
{
    public class WeatherBitService : IWeatherBitService
    {
        private IHttpClientFactory _clientFactory;
        private WeatherBitConfig _config { get; }
        private const string ENDPOINT = "/current";

        public WeatherBitService(IHttpClientFactory clientFactory, IOptions<WeatherBitConfig> options)
        {
            _config = options.Value;
            _clientFactory = clientFactory;
        }

        public async Task<WeatherBitResponseModel> GetWeatherByPostalCode(string postalCode, string include = "minutely", string units = "I")
        {
            var query = new Dictionary<string, string>()
            {
                ["postal_code"] = postalCode,
                ["include"] = include,
                ["units"] = units,
                ["key"] = _config.ApiKey
            };

            var uri = QueryHelpers.AddQueryString(_config.BaseUrl + ENDPOINT, query);

            var client = GetClient();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<WeatherBitResponseModel>(content);
                
            }

            throw new HttpRequestException($"Status code does not indicate a success. StatusCode: {response.StatusCode}");
        }


        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient();
            client.BaseAddress = new Uri(_config.BaseUrl);
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            return client;
        }
    }
}