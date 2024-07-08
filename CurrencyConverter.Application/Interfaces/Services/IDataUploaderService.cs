namespace CurrencyConverter.Application.Interfaces.Services;

public interface IDataUploaderService
{
    Task UploadCurrencyRatesAsync();
}