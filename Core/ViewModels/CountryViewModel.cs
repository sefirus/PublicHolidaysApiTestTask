namespace Core.ViewModels;

public class CountryViewModel
{
    public string CountryCode3Digit { get; set; }
    public string CountryCode2Digit { get; set; }
    public string FullName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}