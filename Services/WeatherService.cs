using Microsoft.Data.SqlClient;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly string _connectionString;

        public WeatherService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task SaveWeatherResultAsync(WeatherResults result)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "INSERT INTO WeatherResults (City, TempMin, TempMax, Humidity, CreatedAt, UserId) VALUES (@City, @TempMin, @TempMax, @Humidity, @CreatedAt, @UserId)", conn);

            cmd.Parameters.AddWithValue("@City", result.City);
            cmd.Parameters.AddWithValue("@TempMin", result.TempMin);
            cmd.Parameters.AddWithValue("@TempMax", result.TempMax);
            cmd.Parameters.AddWithValue("@Humidity", result.Humidity);
            cmd.Parameters.AddWithValue("@CreatedAt", result.CreatedAt);
            cmd.Parameters.AddWithValue("@UserId", result.UserId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

public async Task<List<WeatherResults>> GetAllWeatherResultsAsync()
{
    var results = new List<WeatherResults>();

    using SqlConnection conn = new SqlConnection(_connectionString);
    using SqlCommand cmd = new SqlCommand(@"
        SELECT w.*, u.Username AS CreatedBy
        FROM WeatherResults w
        INNER JOIN Users u ON w.UserId = u.Id
    ", conn);

    await conn.OpenAsync();
    using var reader = await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        results.Add(new WeatherResults
        {
            Id = Convert.ToInt32(reader["Id"]),
            City = reader["City"].ToString() ?? "",
            TempMin = Convert.ToSingle(reader["TempMin"]),
            TempMax = Convert.ToSingle(reader["TempMax"]),
            Humidity = Convert.ToInt32(reader["Humidity"]),
            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
            UserId = Convert.ToInt32(reader["UserId"]),
            CreatedBy = reader["CreatedBy"].ToString() ?? "" 
        });
    }

    return results;
}



public async Task UpdateWeatherResultAsync(WeatherResults result)
{
    using SqlConnection conn = new SqlConnection(_connectionString);
    using SqlCommand cmd = new SqlCommand(
        "UPDATE WeatherResults SET City = @City, TempMin = @TempMin, TempMax = @TempMax, Humidity = @Humidity WHERE Id = @Id", conn);

    cmd.Parameters.AddWithValue("@City", result.City);
    cmd.Parameters.AddWithValue("@TempMin", result.TempMin);
    cmd.Parameters.AddWithValue("@TempMax", result.TempMax);
    cmd.Parameters.AddWithValue("@Humidity", result.Humidity);
    cmd.Parameters.AddWithValue("@Id", result.Id);

    await conn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
}


        public async Task TrackChangeIfDifferent(
            WeatherResults original,
            WeatherResults updated,
            string fieldName,
            string oldVal,
            string newVal,
            int userId)
        {
            if (oldVal != newVal)
            {
                var log = new WeatherChangeLog
                {
                    WeatherResultId = updated.Id,
                    UserId = userId,
                    FieldChanged = fieldName,
                    OldValue = oldVal,
                    NewValue = newVal,
                    ChangeDate = DateTime.UtcNow
                };

                await SaveChangeLogAsync(log);
            }
        }
public async Task SaveChangeLogAsync(WeatherChangeLog log)
{
    using SqlConnection conn = new SqlConnection(_connectionString);
    using SqlCommand cmd = new SqlCommand(
        "INSERT INTO WeatherChangeLog (WeatherResultId, UserId, FieldChanged, OldValue, NewValue, ChangeDate) " +
        "VALUES (@WeatherResultId, @UserId, @FieldChanged, @OldValue, @NewValue, @ChangeDate)", conn);

    cmd.Parameters.AddWithValue("@WeatherResultId", log.WeatherResultId);
    cmd.Parameters.AddWithValue("@UserId", log.UserId);
    cmd.Parameters.AddWithValue("@FieldChanged", log.FieldChanged);
    cmd.Parameters.AddWithValue("@OldValue", log.OldValue);
    cmd.Parameters.AddWithValue("@NewValue", log.NewValue);
    cmd.Parameters.AddWithValue("@ChangeDate", log.ChangeDate);

    await conn.OpenAsync();
    await cmd.ExecuteNonQueryAsync();
}



    }
}
