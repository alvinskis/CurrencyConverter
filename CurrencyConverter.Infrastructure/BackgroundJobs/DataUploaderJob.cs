namespace CurrencyConverter.Infrastructure.BackgroundJobs;

using Application.Interfaces.Services;
using Quartz;

[DisallowConcurrentExecution]
public class DataUploaderJob(IDataUploaderService dataUploaderService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await dataUploaderService.UploadCurrencyRatesAsync();
    }
}