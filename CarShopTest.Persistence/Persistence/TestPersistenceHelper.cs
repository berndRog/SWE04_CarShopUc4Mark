using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CarShop.Di;

namespace CarShopTest.Persistence {

   public static class TestPersistenceHelper {
    
      public static IServiceCollection AddPersistenceTest(
         this IServiceCollection serviceCollection
      ) {

         // Configuration
         // Nuget:  Microsoft.Extensions.Configuration
         //       + Microsoft.Extensions.Configuration.Json
         var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettingsTest.json", false)
            .Build();
         serviceCollection.AddSingleton<IConfiguration>(configuration);

         // Logging
         // Nuget:  Microsoft.Extensions.Logging
         //       + Microsoft.Extensions.Logging.Configuration
         //       + Microsoft.Extensions.Logging.Debug
         var logging = configuration.GetSection("Logging");
         serviceCollection.AddLogging(builder => {
            builder.ClearProviders();
            builder.AddConfiguration(logging);
            builder.AddDebug();
         });
         
         // CarShop.Di: Repository + Database
         serviceCollection.AddPersistence(configuration);

         return serviceCollection;
      }
   }
}