namespace CurrencyConverter.Application.Interfaces.Services;

using DTOs;

public interface ICurrencyConverterService
{
    Task<RateEntryDto> GetLatestCurrencyRates();
}