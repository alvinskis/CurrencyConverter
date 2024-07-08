namespace CurrencyConverter.Application.DTOs;

public record RateEntryDto
{
    public DateOnly Date { get; set; }
    public IEnumerable<CurrencyRateDto> CurrencyRates { get; set; } = [];
};