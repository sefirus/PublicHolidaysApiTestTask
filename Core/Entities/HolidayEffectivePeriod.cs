using Core.Entities;

namespace Core.Enums;

public class HolidayEffectivePeriod
{
    public Guid Id { get; set; }
    public Guid HolidayId { get; set; }
    public Holiday Holiday { get; set; }
        
    public DateTime StartYear { get; set; }
    public DateTime EndYear { get; set; }
}