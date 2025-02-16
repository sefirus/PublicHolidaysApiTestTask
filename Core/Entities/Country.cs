namespace Core.Entities;

public class Country
{
    public Guid Id { get; set; }

    /// <summary>
    /// ISO 3166-1 alpha-3 country code
    /// </summary>
    public string CountryCode3Digit { get; set; }
    
    /// <summary>
    /// ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string CountryCode2Digit { get; set; }

    public string FullName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}