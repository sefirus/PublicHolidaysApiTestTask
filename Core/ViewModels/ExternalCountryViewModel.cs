namespace Core.ViewModels;

public class ExternalCountryViewModel
{
    public string FullName { get; set; }
    public string CountryCode { get; set; }
    public ExternalDateViewModel FromDate { get; set; }
    public ExternalDateViewModel ToDate { get; set; }
}