namespace Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using CurrencyConverter.Application.Interfaces.Persistence;
using CurrencyConverter.Domain.Models;
using Context;

public class CurrencyRateRepository : ICurrencyRateRepository
{
    private readonly CurrencyConverterDbContext _currencyConverterDbContext;

    public CurrencyRateRepository(CurrencyConverterDbContext currencyConverterDbContext)
    {
        ArgumentNullException.ThrowIfNull(currencyConverterDbContext);
        _currencyConverterDbContext = currencyConverterDbContext;
    }

    public async Task<IEnumerable<CurrencyRate>> GetAllByRateEntryIdAsync(int id)
    {
        return await _currencyConverterDbContext.Set<CurrencyRate>()
            .Where(x => x.RateEntry != null && x.RateEntry.Id == id).ToListAsync();
    }
}