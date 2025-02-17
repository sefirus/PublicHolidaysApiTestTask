using Core.Entities;
using Core.Interfaces;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/")]
public class HolidayScheduleController : ControllerBase
{
    private readonly ICountryService _service;
    private readonly IHolidayService _holidayService;
    
    public HolidayScheduleController(ICountryService externalHolidayApiService, IHolidayService holidayService)
    {
        _service = externalHolidayApiService;
        _holidayService = holidayService;
    }

    /// <summary>
    /// Returns list of available countries.
    /// </summary>
    /// <returns>A list of available countries.</returns>
    [HttpGet("countries")]
    [ProducesResponseType(typeof(List<CountryViewModel>), 200)]
    public async Task<IActionResult> Get()
    {
        var countries = await _service.GetAllCountries();
        return Ok(countries);
    }
    
    /// <summary>
    /// Returns holidays for the given country and year, grouped by month.
    /// </summary>
    /// <param name="country">
    /// The country code (ISO 3166-1 alpha-2 or alpha-3).
    /// </param>
    /// <param name="year">The target year.</param>
    /// <returns>A list of month-grouped holidays.</returns>
    [HttpGet("by-month")]
    [ProducesResponseType(typeof(List<MonthHolidaysViewModel>), 200)]
    public async Task<IActionResult> GetHolidaysByMonth([FromQuery] string country, [FromQuery] int year)
    {
        var result = await _holidayService.GetHolidaysGroupedByMonthAsync(country, year);
        return Ok(result);
    }

    /// <summary>
    /// Returns the status of a specific day (WorkDay, FreeDay, or Holiday).
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="date">The date to check.</param>
    /// <returns>A model containing the day and its status.</returns>
    [HttpGet("day-status")]
    [ProducesResponseType(typeof(DayStatusViewModel), 200)]
    public async Task<IActionResult> GetDayStatus([FromQuery] string country, [FromQuery] DateTime date)
    {
        var result = await _holidayService.GetDayStatusAsync(country, date);
        return Ok(result);
    }

    /// <summary>
    /// Returns the maximum number of consecutive free days (weekend + holiday) for the given country and year.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="year">The target year.</param>
    /// <returns>A model containing the maximum number of consecutive free days.</returns>
    [HttpGet("max-free-days")]
    [ProducesResponseType(typeof(MaxFreeDaysViewModel), 200)]
    public async Task<IActionResult> GetMaxFreeDays([FromQuery] string country, [FromQuery] int year)
    {
        var result = await _holidayService.GetMaxConsecutiveFreeDaysAsync(country, year);
        return Ok(result);
    }
}