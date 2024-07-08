using CurrencyConverter.Domain.Models;
using CurrencyConverter.Persistence.UnitTests.Mocks;
using Persistence.Context;
using Persistence.Repositories;

namespace CurrencyConverter.Persistence.UnitTests.Repositories;

public class RateEntryRepositoryTests
{
    [Fact]
    public void ShouldThrowArgumentNullExceptionWhenCurrencyConverterDbContextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new RateEntryRepository(null!));
    }

    [Fact]
    public async Task GetLatestRecordAsyncShouldReturnRateEntryWhenExists()
    {
        var options = DbContextMock.GetInMemoryDbOptions();

        await using (var context = new CurrencyConverterDbContext(options))
        {
            context.RateEntries.AddRange(new List<RateEntry>
            {
                new() { Id = 1, Date = new DateOnly(2024, 01, 01) },
                new() { Id = 2, Date = new DateOnly(2024, 02, 01) }
            });

            await context.SaveChangesAsync();
        }

        await using (var context = new CurrencyConverterDbContext(options))
        {
            var service = new RateEntryRepository(context);

            var result = await service.GetLatestRecordAsync();

            Assert.NotNull(result);
            Assert.Equal(result.Date, new DateOnly(2024, 02, 01));
        }
    }

    [Fact]
    public async Task GetLatestRecordAsyncShouldReturnEmptyWhenNotExists()
    {
        var options = DbContextMock.GetInMemoryDbOptions();

        await using (var context = new CurrencyConverterDbContext(options))
        {
            context.RateEntries.AddRange(new List<RateEntry>());

            await context.SaveChangesAsync();
        }

        await using (var context = new CurrencyConverterDbContext(options))
        {
            var service = new RateEntryRepository(context);

            var result = await service.GetLatestRecordAsync();

            Assert.Null(result);
        }
    }
}