namespace CurrencyConverter.Application.Interfaces.Persistence;

using Domain.Models;

public interface ICurrencyRateRepository
{
    Task<IEnumerable<CurrencyRate>> GetAllByRateEntryIdAsync(int id);
}