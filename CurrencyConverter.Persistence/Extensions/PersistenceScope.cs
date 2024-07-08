namespace Persistence.Extensions;

using Constants;
using CurrencyConverter.Application.Interfaces.Persistence;
using Repositories;
using Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class PersistenceScope
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CurrencyConverterDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(PersistenceConstants.Database)));
        services.AddScoped<IRateEntryRepository, RateEntryRepository>();
        services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();

        return services;
    }
}