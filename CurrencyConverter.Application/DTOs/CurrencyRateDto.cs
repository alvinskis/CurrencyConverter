namespace CurrencyConverter.Application.DTOs;

public record CurrencyRateDto
{
    public string? Currency { get; init; }
    public double ConversionRate { get; init; }
}