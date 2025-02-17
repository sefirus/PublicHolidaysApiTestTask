using System.Runtime.InteropServices.JavaScript;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class HolidayScheduleService : IHolidayScheduleService
{
    private readonly IRepository<Holiday> _holidayRepository;
    private readonly IRepository<ProcessedHolidaysChunk> _chunkRepository;
    private readonly IExternalHolidayApiService _externalApiService;
    private readonly ICountryService _countryService;
    private readonly int _defaultChunkSize;

    public HolidayScheduleService(
        IRepository<Holiday> holidayRepository,
        IRepository<ProcessedHolidaysChunk> chunkRepository,
        IExternalHolidayApiService externalApiService,
        IConfiguration configuration, 
        ICountryService countryService)
    {
        _holidayRepository = holidayRepository;
        _chunkRepository = chunkRepository;
        _externalApiService = externalApiService;
        _countryService = countryService;
        int.TryParse(configuration["ScheduleProcessingChunkSize"], out _defaultChunkSize);
    }
    
    /// <summary>
    /// Determines the effective year boundaries for a given target year and chunk size.
    /// Chunks always start at the year 2000. For example, if chunkSize = 50, then chunks are:
    /// 2000–2049, 2050–2099, 2100–2149, etc.
    /// This method then “clips” the chunk to the country's supported date range:
    ///   commonStart = max(chunkStart, country.FromDate.Year)
    ///   commonEnd   = min(chunkEnd, country.ToDate.Year)
    /// If there is no overlap, it returns null.
    /// </summary>
    public (int CommonStart, int CommonEnd) CalculateEffectiveChunkBoundaries(int targetYear, int chunkSize, Country country)
    {
        // Calculate chunk boundaries based on a fixed start at 2000.
        int offset = targetYear - 2000;
        int chunkIndex = offset / chunkSize; // integer division
        int chunkStartYear = 2000 + chunkIndex * chunkSize;
        int chunkEndYear = chunkStartYear + chunkSize - 1;

        // Clip the chunk to the country's supported dates.
        int commonStart = Math.Max(chunkStartYear, country.FromDate.Year);
        int commonEnd = Math.Min(chunkEndYear, country.ToDate.Year);

        return commonStart > commonEnd ? (0, 0) : (commonStart, commonEnd);
    }

    /// <summary>
    /// Processes a ScheduleProcessingChunkSize-year chunk for a given country.
    /// Pulls data from the external API and stores holiday records.
    /// </summary>
    /// <param name="countryCode">
    /// ISO 3166-1 alpha-3 or alpha-2 country code.
    /// </param>
    /// <param name="targetYear">Year whose chunk should be processed.</param>
    /// <param name="chunkSize">
    /// Optional override for chunk size. If not provided, uses the value from configuration.
    /// </param>
    public async Task ProcessHolidayChunkAsync(string countryCode, int targetYear, int? chunkSize = null)
    {
        var country = await _countryService.GetCountryByCode(countryCode);

        var years = CalculateEffectiveChunkBoundaries(targetYear, chunkSize ?? _defaultChunkSize, country);
        int chunkStartYear = years.CommonStart;
        int chunkEndYear = years.CommonEnd;

        if (chunkEndYear == 0 && chunkStartYear == 0)
        {
            return;
        }
        
         // Check if this chunk has already been processed.
        var existingChunk = (await _chunkRepository.QueryAsync(
            pc => pc.CountryId == country.Id &&
                  pc.ChunkStartYear == chunkStartYear &&
                  pc.ChunkEndYear == chunkEndYear,
            asNoTracking: true)).FirstOrDefault();

        if (existingChunk != null)
        {
            // Chunk already processed, nothing to do.
            return;
        }

        // Fetch external holiday data for the entire chunk.
        var externalHolidays = await _externalApiService.GetHolidays(countryCode, chunkStartYear, chunkEndYear);

        // Group external holidays by holiday type and primary (first) name.
        var groups = externalHolidays.GroupBy(eh => new
        {
            HolidayType = eh.HolidayType,
            PrimaryName = eh.Name.First(n => n.Lang == "en")?.Text
        });

        // Pre-fetch all existing holidays for this country.
        var existingHolidays = (await _holidayRepository.QueryAsync(h => h.CountryId == country.Id)).ToList();
        // Build a lookup dictionary keyed by composite key "HolidayType|PrimaryName".
        var holidayLookup = existingHolidays.ToDictionary(
            h => $"{h.HolidayType}|{h.Names.First(n => n.Lang == "en").Text}",
            h => h);

        // Process each group.
        foreach (var group in groups)
        {
            string groupKey = $"{group.Key.HolidayType}|{group.Key.PrimaryName}";

            // Get or create the holiday record.
            if (!holidayLookup.TryGetValue(groupKey, out Holiday holiday))
            {
                holiday = new Holiday
                {
                    Id = Guid.NewGuid(),
                    CountryId = country.Id,
                    HolidayType = MapHolidayType(group.Key.HolidayType),
                    Names = group.First().Name.Select(n => new LocalizedTextEntry()
                    {
                        Id = Guid.NewGuid(),
                        Lang = n.Lang,
                        Text = n.Text
                    }).ToList(),
                    Occurrences = new List<HolidayOccurrence>()
                };

                await _holidayRepository.Insert(holiday);
                holidayLookup.Add(groupKey, holiday);
            }

            // For each external holiday record in this group, store its occurrence.
            foreach (var ext in group)
            {
                DateTime occurrenceDate = new DateTime(ext.Date.Year < DateTime.MaxValue.Year 
                        ? ext.Date.Year 
                        : DateTime.MaxValue.Year, 
                    ext.Date.Month, 
                    ext.Date.Day);
                // Only add if not already stored.
                if (!holiday.Occurrences.Any(o => o.Date == occurrenceDate))
                {
                    holiday.Occurrences.Add(new HolidayOccurrence
                    {
                        Id = Guid.NewGuid(),
                        HolidayId = holiday.Id,
                        Date = occurrenceDate
                    });
                }
            }
        }

        // Mark the chunk as processed.
        var processedChunk = new ProcessedHolidaysChunk
        {
            Id = Guid.NewGuid(),
            CountryId = country.Id,
            ChunkStartYear = chunkStartYear,
            ChunkEndYear = chunkEndYear,
            ProcessedDate = DateTime.UtcNow
        };
        await _chunkRepository.Insert(processedChunk);

        await _holidayRepository.SaveChangesAsync();
        await _chunkRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Maps the external holiday type string to our HolidayType.
    /// </summary>
    private HolidayType MapHolidayType(string externalType)
    {
        return externalType.ToLowerInvariant() switch
        {
            "public_holiday"    => HolidayType.PublicHoliday,
            "observance"        => HolidayType.Observance,
            "school_holiday"    => HolidayType.SchoolHoliday,
            "other_day"         => HolidayType.OtherDay,
            "extra_working_day" => HolidayType.ExtraWorkingDay,
            _                   => HolidayType.PublicHoliday,
        };
    }
}