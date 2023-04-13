using SeaBrief.DotEnv;
using SeaBrief.MQTT;

class Application {
    static void Main(string[] args)
    {
        try {
            DotEnv.Load(new string[] { "MQTT_ADDRESS", "MQTT_PORT", "MQTT_SERVICE_NAME" });

            var service = Environment.GetEnvironmentVariable("MQTT_SERVICE_NAME")!;
            var address = Environment.GetEnvironmentVariable("MQTT_ADDRESS")!;
            var port = Environment.GetEnvironmentVariable("MQTT_PORT")!;

            MQTTClientSingleton.Instance.Connect(service, address, port);


            Scheduler scheduler = new Scheduler();
            scheduler.Start().Wait();

            scheduler.ScheduleJob<DiskSpace>(TimeSpan.FromMinutes(1)).Wait();
            scheduler.ScheduleJob<Heartbeat>(TimeSpan.FromMinutes(1)).Wait();

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
