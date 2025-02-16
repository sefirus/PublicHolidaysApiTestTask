namespace Core.Exceptions;

public class CountryNotFoundExceptions : Exception
{
    public CountryNotFoundExceptions(string? message) : base(message) { }

}