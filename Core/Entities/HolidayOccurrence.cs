using Core.Entities;

namespace Core.Enums;

public class HolidayOccurrence
{
    public Guid Id { get; set; }
    public Guid HolidayId { get; set; }
    public Holiday Holiday { get; set; }
        
    public DateTime Date { get; set; }
}