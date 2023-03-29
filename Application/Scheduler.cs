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
 
        var job = JobBuilder.Create<TJob>()
            .WithIdentity($"{typeof(TJob).Name}.job")
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{typeof(TJob).Name}.trigger")
            .WithSimpleSchedule(x => x
                .WithInterval(interval)
                .RepeatForever())
            .Build();

        var scheduler = await this.factory.GetScheduler();
        await scheduler.ScheduleJob(job, trigger);
    }
}