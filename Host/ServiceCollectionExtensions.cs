using Application.Services;
using Core.Interfaces;

namespace PublicHolidaysApiTestTask;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExternalHolidayApiService, ExternalHolidayApiService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IHolidayScheduleService, HolidayScheduleService>();
        services.AddScoped<IHolidayService, HolidayService>();
    }
}