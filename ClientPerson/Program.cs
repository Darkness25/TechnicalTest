namespace ClientPerson
{
    public class Program
    {
        public static void Main(string[] args) { CreateHostBuilder(args).Build().Run(); }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(c => { c.AddJsonFile("settings/appsettings.json", optional: true, reloadOnChange: true); })
            .ConfigureLogging(logging =>
            {
                System.Environment.SetEnvironmentVariable("Log4NetFilename", System.Net.Dns.GetHostName());
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddLog4Net("Logger.config");
            })
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}