using Quartz;
using Quartz.Impl;

public class Scheduler
{
    private readonly ISchedulerFactory factory;
    public Scheduler()
    {
        this.factory = new StdSchedulerFactory();
    }

    public async Task Start()
    {
        var scheduler = await this.factory.GetScheduler();
        await scheduler.Start();
    }

    public async Task ScheduleJob<TJob>(TimeSpan interval) where TJob : IJob
    {
        var onStartupJob = JobBuilder.Create<TJob>()
    .WithIdentity($"{typeof(TJob).Name}.startup.job")
    .Build();

        var onIntervalJob = JobBuilder.Create<TJob>()
            .WithIdentity($"{typeof(TJob).Name}.interval.job")
            .Build();

        var onStartupTrigger = TriggerBuilder.Create()
            .WithIdentity($"{typeof(TJob).Name}.startup.trigger")
            .StartNow()
            .Build();

        var onIntervalTrigger = TriggerBuilder.Create()
            .WithIdentity($"{typeof(TJob).Name}.interval.trigger")
            .StartAt(DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow.AddMinutes(1)))
            .WithSimpleSchedule(x => x
                .WithInterval(interval)
                .RepeatForever())
            .Build();

        var scheduler = await this.factory.GetScheduler();
        await scheduler.ScheduleJob(onStartupJob, onStartupTrigger);
        await scheduler.ScheduleJob(onIntervalJob, onIntervalTrigger);
    }
}