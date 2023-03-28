using System.Text;
using Quartz;
using SeaBrief.MQTT.Message;

public class Heartbeat : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var client = Environment.GetEnvironmentVariable("MQTT_CLIENT_ID")!;
        var service = Environment.GetEnvironmentVariable("MQTT_SERVICE_NAME")!;
        new MessageBuilder()
        .WithTopic($"Edge/{client}/{service}/Heartbeat")
        .WithPayload(Encoding.UTF8.GetBytes(DateTime.Now.ToString()))
        .Publish()
        .Wait();

        return Task.CompletedTask;
    }
}