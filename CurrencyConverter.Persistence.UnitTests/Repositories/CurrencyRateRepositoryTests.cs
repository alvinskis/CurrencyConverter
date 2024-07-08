using CurrencyConverter.Domain.Models;
using CurrencyConverter.Persistence.UnitTests.Mocks;
using Persistence.Context;
using Persistence.Repositories;

namespace CurrencyConverter.Persistence.UnitTests.Repositories;

public class CurrencyRateRepositoryTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenCurrencyConverterDbContextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CurrencyRateRepository(null!));
    }

    [Fact]
    public async Task GetAllByRateEntryIdAsyncShouldReturnCurrencyRatesWhenExists()
    {
        var options = DbContextMock.GetInMemoryDbOptions();

        await using (var context = new CurrencyConverterDbContext(options))
        {
            context.CurrencyRates.AddRange(new List<CurrencyRate>
            {
                new() { Currency = "USD", ConversionRate = 1.1, RateEntry = new RateEntry { Id = 1 } },
                new() { Currency = "JPY", ConversionRate = 1.2, RateEntry = new RateEntry { Id = 2 } }
            });

            await context.SaveChangesAsync();
        }

        await using (var context = new CurrencyConverterDbContext(options))
        {
            var service = new CurrencyRateRepository(context);

            var result = await service.GetAllByRateEntryIdAsync(1);

            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);
        }
    }

    [Fact]
    public async Task GetAllByRateEntryIdAsyncShouldReturnEmptyCollectionWhenNotExists()
    {
        var options = DbContextMock.GetInMemoryDbOptions();

        await using (var context = new CurrencyConverterDbContext(options))
        {
            context.CurrencyRates.AddRange(new List<CurrencyRate>());

            await context.SaveChangesAsync();
        }

        await using (var context = new CurrencyConverterDbContext(options))
        {
            var service = new CurrencyRateRepository(context);

            var result = await service.GetAllByRateEntryIdAsync(1);

            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Empty(resultList);
        }
    }
}