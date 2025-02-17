using Core.Enums;

namespace Core.Entities;

public class Holiday
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    public Country Country { get; set; }
        
    /// <summary>
    /// Holiday type.
    /// </summary>
    public HolidayType HolidayType { get; set; }
        
    /// <summary>
    /// Localized names for the holiday.
    /// </summary>
    public List<LocalizedTextEntry> Names { get; set; } = [];
    
    /// <summary>
    /// For non‑predictable holidays: explicit occurrence dates.
    /// </summary>
    public ICollection<HolidayOccurrence> Occurrences { get; set; } = new List<HolidayOccurrence>();
}