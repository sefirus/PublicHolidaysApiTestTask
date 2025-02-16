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
    /// Indicates how the holiday’s date is determined.
    /// </summary>
    public HolidayPatternType PatternType { get; set; }
    
    /// <summary>
    /// Only for predictable(PatternType = Fixed or Relative) holidays: <br/>
    /// - Fixed: Used along with "Day" property.  <br/>
    /// - Relative: Used along with "WeekOfMonth" + "DayOfWeek" properties.  (e.g. Monday of 2nd Week of <b>May</b>)
    /// </summary>
    public int? Month { get; set; }   
    
    /// <summary>
    /// Only for fixed(PatternType = Fixed) holidays: Used along with "Month" property.
    /// </summary>
    public int? Day { get; set; }    
    
    /// <summary>
    /// Only for relative(PatternType = Relative) holidays: Used along with "Month" + "DayOfWeek" properties. Indicates index of the week in the month (e.g. Monday of <b>2nd Week</b> of May). 
    /// </summary>
    public int? WeekOfMonth { get; set; }  
    
    /// <summary>
    /// Only for relative(PatternType = Relative) holidays: Used along with "Month" + "DayOfWeek" properties. Indicates index of the day in the week (e.g. <b>Monday</b> of 2nd Week of May). 
    /// </summary>
    public int? DayOfWeek { get; set; }      
        
    /// <summary>
    /// Localized names for the holiday.
    /// </summary>
    public List<LocalizedTextEntry> Names { get; set; } = [];

    /// <summary>
    /// For predictable holidays: effective periods (date ranges) during which this rule applies.
    /// </summary>
    public ICollection<HolidayEffectivePeriod> EffectivePeriods { get; set; } = new List<HolidayEffectivePeriod>();

    /// <summary>
    /// For non‑predictable holidays: explicit occurrence dates.
    /// </summary>
    public ICollection<HolidayOccurrence> Occurrences { get; set; } = new List<HolidayOccurrence>();
}