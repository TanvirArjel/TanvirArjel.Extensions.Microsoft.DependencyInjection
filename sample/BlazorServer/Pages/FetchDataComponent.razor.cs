using System;
using System.Threading.Tasks;
using BlazorServer.Models;
using BlazorServer.Services;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorServer.Pages
{
    [TransientService]
    public partial class FetchDataComponent
    {
        private readonly WeatherForecastService _weatherForecastService;

        public FetchDataComponent(WeatherForecastService weatherForecastService)
        {
            _weatherForecastService = weatherForecastService;
        }

        private WeatherForecast[] Forecasts { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Forecasts = await _weatherForecastService.GetForecastAsync(DateTime.Now);
        }
    }
}
