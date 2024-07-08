namespace Persistence.Repositories;

using CurrencyConverter.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CurrencyConverter.Application.Interfaces.Persistence;
using Context;

public class RateEntryRepository : IRateEntryRepository
{
    private readonly CurrencyConverterDbContext _currencyConverterDbContext;

    public RateEntryRepository(CurrencyConverterDbContext currencyConverterDbContext)
    {
        ArgumentNullException.ThrowIfNull(currencyConverterDbContext);
        _currencyConverterDbContext = currencyConverterDbContext;
    }

    public async Task AddAsync(RateEntry rateEntry)
    {
        await _currencyConverterDbContext.AddAsync(rateEntry);
        await _currencyConverterDbContext.SaveChangesAsync();
    }

    public async Task<RateEntry?> GetLatestRecordAsync()
    {
        return (await _currencyConverterDbContext.Set<RateEntry>()
            .Where(x => true)
            .OrderByDescending(x => x.Date)
            .FirstOrDefaultAsync())!;
    }
}