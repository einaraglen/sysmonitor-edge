using SeaBrief.DotEnv;
using SeaBrief.MQTT;

class Application {
    static void Main(string[] args)
    {
        try {
            DotEnv.Load(new string[] { "MQTT_CLIENT_ID", "MQTT_ADDRESS", "MQTT_PORT", "MQTT_SERVICE_NAME" });

            var client = Environment.GetEnvironmentVariable("MQTT_CLIENT_ID")!;
            var service = Environment.GetEnvironmentVariable("MQTT_SERVICE_NAME")!;
            var address = Environment.GetEnvironmentVariable("MQTT_ADDRESS")!;
            var port = Environment.GetEnvironmentVariable("MQTT_PORT")!;

            MQTTClientSingleton.Instance.Connect($"RemoteEdge@{client}", address, port);


            Scheduler scheduler = new Scheduler();
            scheduler.Start().Wait();

            scheduler.ScheduleJob<DiskSpace>(TimeSpan.FromHours(4)).Wait();
            scheduler.ScheduleJob<Heartbeat>(TimeSpan.FromMinutes(10)).Wait();

            Console.ReadLine();
        } catch (Exception ex) {
            Console.WriteLine(ex);
        }
        finally
        {
            MQTTClientSingleton.Instance.Disconnect();
        }
    }
}
