using Core.ViewModels;

namespace Core.Interfaces;

public interface IHolidayService
{
    /// <summary>
    /// Returns a list of holidays for the given country and year, grouped by month.
    /// </summary>
    Task<List<MonthHolidaysViewModel>> GetHolidaysGroupedByMonthAsync(string countryCode, int year);

    /// <summary>
    /// Returns the day status (WorkDay, FreeDay, or Holiday) for the given date.
    /// </summary>
    Task<DayStatusViewModel> GetDayStatusAsync(string countryCode, DateTime date);

    /// <summary>
    /// Returns the maximum number of consecutive free days (free day or holiday) for the given country and year.
    /// </summary>
    Task<MaxFreeDaysViewModel> GetMaxConsecutiveFreeDaysAsync(string countryCode, int year);
}