using API.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace RabbitMQBackgroundService
{
    public class Program

    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<RedisService>(sp =>
                    {
                        var configuration = sp.GetRequiredService<IConfiguration>();
                        return new RedisService(configuration);
                    });

                });



    };
      


    
}