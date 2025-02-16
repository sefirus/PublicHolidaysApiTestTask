namespace Core.Entities;

public class LocalizedTextEntry
{
    public Guid Id { get; set; }
    public Holiday Holiday { get; set; }
    public Guid HolidayId { get; set; }
    public string Lang { get; set; }
    public string Text { get; set; }
}