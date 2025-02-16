using Core.ViewModels;

namespace Core.Interfaces;

public interface IExternalHolidayApiService
{
    /// <summary>
    /// Retrieves holiday data from the external API for the given country and year range.
    /// </summary>
    Task<List<ExternalHolidayViewModel>> GetHolidaysAsync(string countryCode, int startYear, int endYear);
}