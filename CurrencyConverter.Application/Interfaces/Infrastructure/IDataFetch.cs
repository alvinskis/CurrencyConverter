namespace CurrencyConverter.Application.Interfaces.Infrastructure;

using DTOs;

public interface IDataFetch
{
    Task<RateEntryDto> FetchDataAsync();
}