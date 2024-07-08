namespace CurrencyConverter.Infrastructure.Extensions;

using CurrencyConverter.Application.Interfaces.Infrastructure;
using Configs;
using Constants;
using Microsoft.Extensions.Options;
using BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

public static class InfrastructureScope
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<FetchDataOptions>().BindConfiguration(InfrastructureConstants.FetchDataOptions);
        services.AddScoped<IDataFetch, XmlDataFetchService.XmlDataFetchService>();

        services.AddQuartz(options =>
        {
            var jobKey = JobKey.Create(nameof(DataUploaderJob));
            var fetchDataOptions =
                services.BuildServiceProvider().GetRequiredService<IOptions<FetchDataOptions>>().Value;

            options.AddJob<DataUploaderJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey).WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInHours(fetchDataOptions.FrequencyInHours).RepeatForever()));
        });
        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
}