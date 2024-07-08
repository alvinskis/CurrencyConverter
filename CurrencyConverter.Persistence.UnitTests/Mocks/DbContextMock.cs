using Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.Persistence.UnitTests.Mocks;

public static class DbContextMock
{
    public static DbContextOptions<CurrencyConverterDbContext> GetInMemoryDbOptions()
    {
        return new DbContextOptionsBuilder<CurrencyConverterDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
    }
}