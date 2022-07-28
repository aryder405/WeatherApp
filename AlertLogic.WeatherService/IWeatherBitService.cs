using AlertLogic.WeatherService.models;

namespace AlertLogic.WeatherService
{
    public interface IWeatherBitService
    {
        Task<WeatherBitResponseModel> GetWeatherByPostalCode(string postalCode, string include = "minutely", string units = "I");
    }
}
