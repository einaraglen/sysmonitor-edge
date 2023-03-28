using System.Text;
using Quartz;
using SeaBrief.MQTT.Message;
public class DiskSpace : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();
        Dictionary<string, long[]> diskspace = new Dictionary<string, long[]>();
        foreach (DriveInfo d in allDrives)
        {
            diskspace[d.Name] = new long[]{ d.TotalFreeSpace, d.TotalSize };
        }

        var client = Environment.GetEnvironmentVariable("MQTT_CLIENT_ID")!;
        var service = Environment.GetEnvironmentVariable("MQTT_SERVICE_NAME")!;
        new MessageBuilder()
        .WithTopic($"Edge/{client}/{service}/DiskSpace")
        .WithPayload(Encoding.UTF8.GetBytes(DateTime.Now.ToString()))
        .Publish()
        .Wait();

        return Task.CompletedTask;
    }
}