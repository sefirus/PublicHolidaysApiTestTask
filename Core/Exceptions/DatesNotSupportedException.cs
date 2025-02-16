namespace Core.Exceptions;

public class DatesNotSupportedException : Exception
{
    public DatesNotSupportedException(string? message) : base(message) { }
}