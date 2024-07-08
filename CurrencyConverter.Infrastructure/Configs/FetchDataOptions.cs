namespace CurrencyConverter.Infrastructure.Configs;

public record FetchDataOptions
{
    public string? DataUrl { get; init; }
    public int FrequencyInHours { get; init; }
}