using System.Text.Json.Serialization;

public class OpenWeatherMapResponse
{
    public List<WeatherDescription> Weather { get; set; }
    [JsonPropertyName("main")]
    public MainInfo Main { get; set; }
}
 public class WeatherDescription
    {
        public string Main { get; set; }  
        public string Description { get; set; }
        public string Icon { get; set; }
    }

public class MainInfo
{
    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
}
