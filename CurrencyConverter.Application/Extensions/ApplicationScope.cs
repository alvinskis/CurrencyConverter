namespace CurrencyConverter.Application.Extensions;

using System.Reflection;
using CurrencyConverter.Application.Interfaces.Services;
using Services;
using Microsoft.Extensions.DependencyInjection;

public static class ApplicationScope
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
        services.AddScoped<IDataUploaderService, DataUploaderService>();

        return services;
    }
}