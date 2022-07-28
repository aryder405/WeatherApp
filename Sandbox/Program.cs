// See https://aka.ms/new-console-template for more information
using AlertLogic.WeatherService;
using AlertLogic.WeatherService.models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sandbox;
using System.Text.RegularExpressions;

var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile($"appsettings.json");
var config = configuration.Build();

using IHost host = Host.CreateDefaultBuilder(args)    
    .ConfigureServices((context, services) =>
    {
        var section = config.GetSection("WeatherBitOptions");
        services.AddOptions<WeatherBitConfig>().Bind(section);
        services.AddHttpClient();
        services.AddScoped<IWeatherBitService, WeatherBitService>();
        services.AddScoped<AlertLogicTasks>();
    })
    .ConfigureLogging(x => {
        x.SetMinimumLevel(LogLevel.Error);
    })
    .Build();

var weatherService = host.Services.GetService<IWeatherBitService>();

Console.WriteLine("Starting application...");

await Run(host);

await host.RunAsync();


async static Task Run(IHost host)
{
    try
    {
        var zipCodes = GetUserInput();

        var tasks = new List<Task>();

        var results = new List<AlertLogicModel>();

        var alertLogicTasks = host.Services.GetService<AlertLogicTasks>();

        foreach (var zipCode in zipCodes)
        {
            var task = alertLogicTasks.ProcessZipCode(zipCode, results);

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        var fileName = $"weather-{DateTime.Now.ToString("hh-mm-ss")}.txt";

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);

        var headerRow = GetHeader<AlertLogicModel>();
        await File.WriteAllTextAsync(filePath, $"{headerRow}\n");
        await File.AppendAllLinesAsync(filePath, results.Select(x => GetValues<AlertLogicModel>(x)));

        Console.WriteLine($"File saved to {filePath}");

        await Run(host);
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Environment.Exit(0);
    }
}


static IEnumerable<string> GetUserInput()
{
    Console.WriteLine("Enter zip codes (comma separated) or 'exit' to terminate");

    var zipCode = Console.ReadLine();

    if(zipCode.ToLower() == "exit")
    {
        Console.WriteLine("Terminating application...");
        Environment.Exit(0);
    }

    if (string.IsNullOrWhiteSpace(zipCode) || zipCode == "-h")
    {
        Console.WriteLine("Enter one or more zip codes to retrieve weather information.\nThe data will be printed to a file in the current directory.");
        return GetUserInput();
    }

    return zipCode.Split(',').Select(x => x.Trim()).Where(x => IsValidUsZipCode(x));
}

static bool IsValidUsZipCode(string zip)
{
    var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
    var valid = Regex.IsMatch(zip, _usZipRegEx);
    if (!valid)
    {
        Console.WriteLine($"Invalid US ZipCode: {zip}");
    }
    return valid;
}

static string GetHeader<T>()
{
    var props = typeof(T).GetProperties().Select(x => x.Name);

    return String.Join('\t', props);
}

static string GetValues<T>(T model)
{
    var values = typeof(T).GetProperties().Select(x => x.GetValue(model));

    return String.Join('\t', values);
}