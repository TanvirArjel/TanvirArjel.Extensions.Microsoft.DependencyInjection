using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorWasm.Models;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorWasm.Services
{
    [ScopedService]
    public class WeatherForecastService
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            WeatherForecast[] weatherForecasts = await _httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
            return weatherForecasts;
        }
    }
}
