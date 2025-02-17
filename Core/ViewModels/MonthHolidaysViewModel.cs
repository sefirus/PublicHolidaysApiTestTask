namespace Core.ViewModels;

public class MonthHolidaysViewModel
{
    public int Month { get; set; }
    public List<HolidayItemViewModel> Holidays { get; set; }
}