namespace CurrencyConverter.Domain.Models;

public record CurrencyRate
{
    public int Id { get; init; }
    public RateEntry? RateEntry { get; init; }
    public string? Currency { get; init; }
    public double ConversionRate { get; init; }
}