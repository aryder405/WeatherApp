using AlertLogic.WeatherService;
using AlertLogic.WeatherService.models;

namespace Sandbox
{
    public class AlertLogicTasks
    {
        private readonly IWeatherBitService _weatherBitService;

        public AlertLogicTasks(IWeatherBitService weatherBitService)
        {
            _weatherBitService = weatherBitService;
        }

        async public Task ProcessZipCode(string zipCode, List<AlertLogicModel> results)
        {
            var response = await _weatherBitService.GetWeatherByPostalCode(zipCode);

            var output = GetAlertLogicModel(zipCode, response);

            results.Add(output);
        }

        private AlertLogicModel GetAlertLogicModel(string zipCode, WeatherBitResponseModel model)
        {
            var dataModel = model.WeatherData?.FirstOrDefault();

            if (dataModel is null)
            {
                throw new Exception("Weather data is empty");
            }

            var result = new AlertLogicModel();
            result.ZipCode = zipCode;
            result.TimeZone = dataModel.TimeZone;
            result.City = dataModel.City;
            result.State = dataModel.State;
            result.WeatherDescription = dataModel.Weather.Description;
            result.CurrentPrecipPercent = string.Format("{0:0.00}", dataModel.Precipitation);
            result.CurrentTempInFahrenheit = string.Format("{0:0.00}", dataModel.Temperature);

            if (model.WeatherMinutely != null && model.WeatherMinutely.Any())
            {
                result.AverageTempInFahrenheit = string.Format("{0:0.00}", model.WeatherMinutely.Average(x => x.Temperature));
                result.AveragePrecipPercent = string.Format("{0:0.00}", model.WeatherMinutely.Average(x => x.Precipitation));
                result.StartTime = model.WeatherMinutely.Min(x => x.DateTime).ToString();
                result.EndTime = model.WeatherMinutely.Max(x => x.DateTime).ToString();
            }

            return result;
        }

    }
}
