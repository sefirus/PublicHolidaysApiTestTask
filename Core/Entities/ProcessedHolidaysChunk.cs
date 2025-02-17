namespace Core.Entities;

/// <summary>
/// Records that a particular year chunk for a given country has been processed and stored in the DB.
/// </summary>
public class ProcessedHolidaysChunk
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
    public int ChunkStartYear { get; set; }
    public int ChunkEndYear { get; set; }
    public DateTime ProcessedDate { get; set; }
}