namespace CurrencyConverter.Domain.Models;

public record RateEntry
{
    public int Id { get; init; }
    public DateOnly Date { get; init; }
    public ICollection<CurrencyRate> CurrencyRates { get; init; } = new List<CurrencyRate>();
};