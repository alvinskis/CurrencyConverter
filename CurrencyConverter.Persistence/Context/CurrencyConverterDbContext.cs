namespace Persistence.Context;

using CurrencyConverter.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class CurrencyConverterDbContext(DbContextOptions<CurrencyConverterDbContext> options) : DbContext(options)
{
    public DbSet<RateEntry> RateEntries { get; init; }
    public DbSet<CurrencyRate> CurrencyRates { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RateEntry>()
            .HasIndex(e => e.Date)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}