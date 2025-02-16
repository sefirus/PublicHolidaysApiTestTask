namespace Core.Enums;

public enum HolidayPatternType
{
    /// <summary>
    /// Occurs every year on the same month and day
    /// </summary>
    Fixed,      
    /// <summary>
    /// Occurs on an ordinal day of the week of a month.
    /// </summary>
    Relative,   
    /// <summary>
    /// Non‑predictable; store explicit occurrences.
    /// </summary>
    Special     
}