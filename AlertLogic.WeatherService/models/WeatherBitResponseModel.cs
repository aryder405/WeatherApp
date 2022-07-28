using System.Text.Json.Serialization;

namespace AlertLogic.WeatherService.models
{
    public class WeatherBitResponseModel
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<WeatherBitModel>? WeatherData { get; set; }
        
        [JsonPropertyName("minutely")]
        public IEnumerable<WeatherBitModel>? WeatherMinutely { get; set; }
    }
}
