using System;
using System.Threading.Tasks;
using BlazorWasm.Models;
using BlazorWasm.Services;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorWasm.Pages
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
