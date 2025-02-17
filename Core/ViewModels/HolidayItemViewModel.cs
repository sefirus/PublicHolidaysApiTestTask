namespace Core.ViewModels;

public class HolidayItemViewModel
{
    public DateTypeViewModel Date { get; set; }
    public List<ExternalLocalizedTextViewModel> Names { get; set; }
    public string HolidayType { get; set; }
}