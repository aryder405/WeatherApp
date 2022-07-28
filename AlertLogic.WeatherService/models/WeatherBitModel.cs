using System.Text.Json.Serialization;

namespace AlertLogic.WeatherService.models
{
    public class WeatherBitModel
    {
        [JsonPropertyName("timestamp_local")]
        public DateTime? DateTime { get; set; }

        [JsonPropertyName("timezone")]
        public string? TimeZone { get; set; }

        [JsonPropertyName("city_name")]
        public string? City { get; set; }
        
        [JsonPropertyName("state_code")]
        public string? State { get; set; }

        [JsonPropertyName("temp")]
        public double? Temperature { get; set; }

        [JsonPropertyName("app_temp")]
        public double? TemperatureApparent { get; set; }

        [JsonPropertyName("precip")]
        public double? Precipitation { get; set; }

        [JsonPropertyName("weather")]
        public WeatherModel Weather { get; set; } = new WeatherModel();


        public partial class WeatherModel
        {
            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
