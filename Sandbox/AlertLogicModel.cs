using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class AlertLogicModel
    {
        public string? ZipCode { get; set; }
        public string? TimeZone { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? WeatherDescription { get; set; }
        public string? CurrentPrecipPercent { get; set; }
        public string? CurrentTempInFahrenheit { get; set; }
        public string? AveragePrecipPercent { get; set; }
        public string? AverageTempInFahrenheit { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
    }
}
