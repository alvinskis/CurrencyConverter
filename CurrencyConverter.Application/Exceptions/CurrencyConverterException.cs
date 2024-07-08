namespace CurrencyConverter.Application.Exceptions;

public class CurrencyConverterException : Exception
{
    public CurrencyConverterException()
    {
    }

    public CurrencyConverterException(string message) : base(message)
    {
    }
}