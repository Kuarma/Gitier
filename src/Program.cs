using Serilog;

namespace Gitier;

public static class Program
{
  public static async Task Main(string[] args)
  {
    await CreateHostBuilder(args)
            .Build()
            .RunAsync();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureServices((hostcontext, services) =>
              {
                services.AddSerilog(loggerConfiguration =>
                    loggerConfiguration
                      .Enrich.FromLogContext()
                      .WriteTo.Console()
                      .CreateLogger());

                services.AddHostedService<Worker>();
              });
}