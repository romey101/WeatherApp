namespace WeatherApp.Models
{
    public class WeatherResults
    {
        public int Id { get; set; }
        public string City { get; set; }
        public float TempMin { get; set; }
        public float TempMax { get; set; }
        public int Humidity { get; set; }

        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string? CreatedBy { get; set; }


    }
public class WeatherChangeLog
{
    public int Id { get; set; }
    public int WeatherResultId { get; set; }
    public int UserId { get; set; }
    public string FieldChanged { get; set; } = string.Empty;
    public string OldValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime ChangeDate { get; set; }
}

}

