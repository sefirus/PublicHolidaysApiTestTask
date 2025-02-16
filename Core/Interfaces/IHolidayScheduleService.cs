namespace Core.Interfaces;

public interface IHolidayScheduleService
{
    /// <summary>
    /// Processes a ScheduleProcessingChunkSize(from IConfiguration)-year chunk for a given country.
    /// Pulls data from external Api, analyzes the schedule, and stores/updates holiday records.
    /// </summary>
    /// <param name="countryCode">ISO 3166-1 alpha-3 country code or ISO 3166-1 alpha-2 country code.</param>
    /// <param name="targetYear">Year, whose chunk should be processed</param>
    /// <param name="chunkSize">Chunk size to process. Overrides ScheduleProcessingChunkSize value from configuration</param>
    /// <returns></returns>
    Task ProcessHolidayChunkAsync(string countryCode, int targetYear, int? chunkSize = null);
}