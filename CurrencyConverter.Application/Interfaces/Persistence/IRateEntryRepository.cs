namespace CurrencyConverter.Application.Interfaces.Persistence;

using Domain.Models;

public interface IRateEntryRepository
{
    Task AddAsync(RateEntry rateEntry);
    Task<RateEntry?> GetLatestRecordAsync();
}