using Microsoft.EntityFrameworkCore;

namespace ClientPerson
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.Configure<ServiceSettings>(Configuration.GetSection("ServiceSettings"));            

            IServiceSettings serviceSettings = new ServiceSettings();
            Configuration.Bind("ServiceSettings", serviceSettings);
            services.AddSingleton(serviceSettings);

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            string connectionString = serviceSettings.TechnicalTest.ConnectionStrings.ConnectionString;
            services.AddDbContext<ClientsContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddScoped<IClientService, ClientService>();

            services.AddHealthChecks();
            services.AddOptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz");
                endpoints.MapGet("/version", async context =>
                {
                    Version ver = new();
                    string version = Environment.GetEnvironmentVariable("VERSION");

                    if (!string.IsNullOrEmpty(version)) ver = Version.Parse(version.Replace("v", string.Empty));
                    else ver = typeof(Program).Assembly.GetName().Version;

                    await context.Response.WriteAsync(string.Format("{0}.{1}.{2}", ver.Major, ver.Minor, ver.Build));
                });
                endpoints.MapControllers();
            });
        }
    }
}
