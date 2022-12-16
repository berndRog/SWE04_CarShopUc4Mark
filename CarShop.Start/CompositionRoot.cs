using CarShop.Di;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CarShop.Start;
class CompositionRoot {

   #region fields
   public static readonly string env =
       Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
   private readonly IHost _host = default!;
   private readonly ILogger<CompositionRoot> _logger = default!;
   #endregion

   #region ctor
   public CompositionRoot() {
      _host = Host.CreateDefaultBuilder()
      .ConfigureAppConfiguration((context, configuration) => {
         configuration.SetBasePath(context.HostingEnvironment.ContentRootPath);
         configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
         configuration.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
      })
      .ConfigureLogging((context, logging) => {
         logging.ClearProviders();
         logging.AddConfiguration(context.Configuration.GetSection("Logging"));
         logging.AddDebug();
      })
      .ConfigureServices((context, services) => {
         services.AddOptions();
         services.AddLogging(builder => builder.AddDebug());
         services.AddPresentation();
         services.AddApplication();
         services.AddPersistence(context.Configuration);
      })
      .Build()
         ?? throw new ApplicationException("Creating the Host failed");

      _logger = _host.Services.GetService<ILogger<CompositionRoot>>()
         ?? throw new ApplicationException("Creating Logger<App> failed");
      _logger.LogDebug("Host created Environment {end}", env);
      _logger.LogDebug("Ctor() {hashcode}", GetHashCode());
   }
   #endregion

   void RunWithDiContainer() {
      var dialog = _host.Services.GetService<IDialog>();
      dialog?.ShowDialog();
   }

   static void Main() {
      new CompositionRoot().RunWithDiContainer();
      Console.ReadKey();
   }
}