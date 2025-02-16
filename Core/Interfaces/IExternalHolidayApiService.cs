using Core.ViewModels;

namespace Core.Interfaces;

public interface IExternalHolidayApiService
{
    /// <summary>
    /// Retrieves countries data from the external API.
    /// </summary>
    Task<List<ExternalCountryViewModel>> GetCountries();
    
    /// <summary>
    /// Retrieves holiday data from the external API for the given country and year range.
    /// </summary>
    Task<List<ExternalHolidayViewModel>> GetHolidays(string countryCode, int startYear, int endYear);
}