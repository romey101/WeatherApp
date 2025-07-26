using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using WeatherApp.Models;
using WeatherApp.Services;


namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly WeatherService _weatherService;
        private readonly IConfiguration _config;

        private void PopulateSaudiCities()
{
    ViewBag.Cities = new List<string>
    {
        "Riyadh", "Jeddah", "Mecca", "Medina", "Dammam", "Khobar", "Taif",
        "Tabuk", "Buraidah", "Abha", "Khamis Mushait", "Najran", "Jazan",
        "Al Hofuf", "Hail", "Al Qassim", "Al Mubarraz", "Yanbu", "Al Baha",
        "Arar", "Sakaka", "Rafha", "Qurayyat", "Al Jawf", "Dhahran"
    };
}


        public WeatherController(IHttpClientFactory httpClientFactory, WeatherService weatherService, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _weatherService = weatherService;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            PopulateSaudiCities();
            return View(new WeatherResults());
        }

private string GetBackgroundClass(string weatherMain)
{
    return weatherMain.ToLower() switch
    {
        "clear" => "sunny",
        "clouds" => "cloudy",
        "rain" => "rainy",
        "thunderstorm" => "stormy",
        "snow" => "snowy",
        _ => "default"
    };
}


        [HttpPost]
public async Task<IActionResult> GetWeather(string city)
{
    PopulateSaudiCities();

    if (string.IsNullOrWhiteSpace(city))
    {
        ViewBag.Error = "Please select a city.";
        return View("Index");
    }


    var apiKey = _config["OpenWeatherMap:ApiKey"];
    var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
    var client = _httpClientFactory.CreateClient();
    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        ViewBag.Error = "City not found or API error.";
        return View("Index");
    }

    var json = await response.Content.ReadAsStringAsync();
    var weather = JsonSerializer.Deserialize<OpenWeatherMapResponse>(json, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

            if (weather?.Main == null || weather?.Weather == null || !weather.Weather.Any())
            {
                ViewBag.Error = "Failed to parse weather data.";
                return View("Index");
            }

int? userId = HttpContext.Session.GetInt32("userId");
    if (userId == null || userId == 0)
    {
        TempData["Error"] = "You must be logged in to get weather info.";
        return RedirectToAction("Login", "Account");
    }

            ViewBag.Background = GetBackgroundClass(weather.Weather[0].Main);
ViewBag.WeatherCondition = weather.Weather[0].Main;

    var result = new WeatherResults
    {
        City = city,
        TempMin = (float)weather.Main.TempMin,
        TempMax = (float)weather.Main.TempMax,
        Humidity = weather.Main.Humidity
    };

    return View("Index", result);
}



[HttpPost]
public async Task<IActionResult> SaveWeatherResult(WeatherResults model)
{
    int? userId = HttpContext.Session.GetInt32("userId");
    if (userId == null || userId == 0)
    {
        TempData["Error"] = "You must be logged in to save weather results.";
        return RedirectToAction("Index", "Account");
    }

    if (string.IsNullOrWhiteSpace(model.City))
    {
        TempData["Error"] = "Please select a city before saving.";
        return RedirectToAction("Index", "Weather");
    }

    model.UserId = userId.Value;
    model.CreatedAt = DateTime.UtcNow;

    await _weatherService.SaveWeatherResultAsync(model);
    return RedirectToAction("Index", "Weather");
}


        [HttpGet]
public async Task<IActionResult> WeatherHistory(string? cityFilter)
{
    var results = await _weatherService.GetAllWeatherResultsAsync();

    if (!string.IsNullOrWhiteSpace(cityFilter))
        results = results
            .Where(r => r.City.Contains(cityFilter, StringComparison.OrdinalIgnoreCase))
            .ToList();

    return View(results); 
}


        [HttpPost]
public async Task<IActionResult> UpdateWeatherResults(List<WeatherResults> updatedResults)
{
    int userId = HttpContext.Session.GetInt32("userId") ?? 0;

    var originalResults = await _weatherService.GetAllWeatherResultsAsync();

    foreach (var updated in updatedResults)
    {
        var original = originalResults.FirstOrDefault(r => r.Id == updated.Id);
        if (original == null) continue;

        await _weatherService.TrackChangeIfDifferent(original, updated, "City", original.City, updated.City, userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "TempMin", original.TempMin.ToString(), updated.TempMin.ToString(), userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "TempMax", original.TempMax.ToString(), updated.TempMax.ToString(), userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "Humidity", original.Humidity.ToString(), updated.Humidity.ToString(), userId);

        await _weatherService.UpdateWeatherResultAsync(updated);
    }

    return RedirectToAction("WeatherHistory");
}

[HttpGet]
public async Task<IActionResult> All()
{
    var results = await _weatherService.GetAllWeatherResultsAsync();
    return View(results); 
}

[HttpPost]
public async Task<IActionResult> SaveAllChanges(List<WeatherResults> updatedResults)
{
    int userId = HttpContext.Session.GetInt32("userId") ?? 0;

    var originalResults = await _weatherService.GetAllWeatherResultsAsync();

    foreach (var updated in updatedResults)
    {
        var original = originalResults.FirstOrDefault(r => r.Id == updated.Id);
        if (original == null) continue;

        await _weatherService.TrackChangeIfDifferent(original, updated, "City", original.City, updated.City, userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "TempMin", original.TempMin.ToString(), updated.TempMin.ToString(), userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "TempMax", original.TempMax.ToString(), updated.TempMax.ToString(), userId);
        await _weatherService.TrackChangeIfDifferent(original, updated, "Humidity", original.Humidity.ToString(), updated.Humidity.ToString(), userId);

        await _weatherService.UpdateWeatherResultAsync(updated);
    }

    return RedirectToAction("All");
}



    }
}
