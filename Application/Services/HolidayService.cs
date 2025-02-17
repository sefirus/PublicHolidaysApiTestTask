using Core.Entities;
using Core.Interfaces;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class HolidayService : IHolidayService
{
    private readonly IHolidayScheduleService _scheduleService;
    private readonly IRepository<Holiday> _holidayRepository;
    private readonly ICountryService _countryService;

    public HolidayService(
        IHolidayScheduleService scheduleService,
        IRepository<Holiday> holidayRepository,
        ICountryService countryService)
    {
        _scheduleService = scheduleService;
        _holidayRepository = holidayRepository;
        _countryService = countryService;
    }

    private async Task EnsureHolidayScheduleExist(string countryCode, int year)
    {
        await _scheduleService.ProcessHolidayChunkAsync(countryCode, year);
    }

    public async Task<List<MonthHolidaysViewModel>> GetHolidaysGroupedByMonthAsync(string countryCode, int year)
    {
        await EnsureHolidayScheduleExist(countryCode, year);

        var country = await _countryService.GetCountryByCode(countryCode, year);

        var holidays = await _holidayRepository
            .QueryAsync(filter: h => h.CountryId == country.Id, 
                include: include => include
                    .Include(h => h.Occurrences.Where(o => o.Date.Year == year))
                    .Include(h => h.Names), 
                asNoTracking: true);

        var occurrences = holidays
            .SelectMany(h => h.Occurrences.Select(o => new { Holiday = h, Occurrence = o }))
            .ToList();

        // Group by month and map to view models.
        var grouped = occurrences
            .GroupBy(x => x.Occurrence.Date.Month)
            .Select(g => new MonthHolidaysViewModel
            {
                Month = g.Key,
                Holidays = g.Select(x => new HolidayItemViewModel
                {
                    Date = new DateTypeViewModel
                    {
                        Day = x.Occurrence.Date.Day,
                        Month = x.Occurrence.Date.Month,
                        Year = x.Occurrence.Date.Year,
                        DayOfWeek = (int)x.Occurrence.Date.DayOfWeek
                    },
                    Names = x.Holiday.Names.Select(n => new ExternalLocalizedTextViewModel() { Lang = n.Lang, Text = n.Text })
                        .ToList(),
                    HolidayType = x.Holiday.HolidayType.ToString()
                }).OrderBy(hi => hi.Date.Day).ToList()
            })
            .OrderBy(g => g.Month)
            .ToList();

        return grouped;
    }

    public async Task<DayStatusViewModel> GetDayStatusAsync(string countryCode, DateTime date)
    {
        await EnsureHolidayScheduleExist(countryCode, date.Year);

        var country = await _countryService.GetCountryByCode(countryCode, date.Year);
        var holidays = await _holidayRepository
            .QueryAsync(filter: h => h.CountryId == country.Id, 
                include: include => include
                    .Include(h => h.Occurrences.Where(o => o.Date == date.Date)),
                asNoTracking: true);

        bool isHoliday = holidays
            .SelectMany(h => h.Occurrences)
            .Any(o => o.Date.Date == date.Date);

        // Define status: if explicitly a holiday then "Holiday". Otherwise, weekends are "FreeDay".
        string status;
        if (isHoliday)
            status = "Holiday";
        else if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            status = "FreeDay";
        else
            status = "WorkDay";

        return new DayStatusViewModel
        {
            Date = date,
            Status = status
        };
    }

    public async Task<MaxFreeDaysViewModel> GetMaxConsecutiveFreeDaysAsync(string countryCode, int year)
    {
        await EnsureHolidayScheduleExist(countryCode, year);

        var country = await _countryService.GetCountryByCode(countryCode, year);
        var holidays = await _holidayRepository
            .QueryAsync(filter: h => h.CountryId == country.Id, 
                include: include => include
                    .Include(h => h.Occurrences.Where(o => o.Date.Year == year)),
                asNoTracking: true);

        // Build a set of all holiday dates in the given year.
        var holidayDates = holidays
            .SelectMany(h => h.Occurrences)
            .Select(o => o.Date.Date)
            .ToHashSet();

        int maxConsecutive = 0;
        int currentConsecutive = 0;
        DateTime start = new DateTime(year, 1, 1);
        DateTime end = new DateTime(year, 12, 31);

        // Loop through each day of the year.
        for (DateTime dt = start; dt <= end; dt = dt.AddDays(1))
        {
            bool isHoliday = holidayDates.Contains(dt);
            bool isWeekend = (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);

            if (isHoliday || isWeekend)
            {
                currentConsecutive++;
                if (currentConsecutive > maxConsecutive)
                    maxConsecutive = currentConsecutive;
            }
            else
            {
                currentConsecutive = 0;
            }
        }

        return new MaxFreeDaysViewModel
        {
            Year = year,
            MaxConsecutiveFreeDays = maxConsecutive
        };
    }
}