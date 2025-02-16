namespace Core.ViewModels;

public class ExternalHolidayViewModel
{
    public ExternalDateViewModel Date { get; set; }
    public List<ExternalLocalizedTextViewModel> Name { get; set; }
    public string HolidayType { get; set; }
}