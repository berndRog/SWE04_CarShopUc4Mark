using CarShop.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Di;
public static class ServicesApplication {
      public static IServiceCollection AddPersistence(
      this IServiceCollection serviceCollection,
      IConfiguration configuration
   ) {

      serviceCollection.AddSingleton<IUnitOfWork,CUnitOfWork>();

      var (useDatabase, dataSource) = CDbSelection.CreateDataSource(configuration);

      switch (useDatabase) {
         case "LocalDb":
         case "SqlServer":
            serviceCollection.AddDbContext<CDbContext>(o => o.UseSqlServer(dataSource));
            break;
         case "Sqlite":
            serviceCollection.AddDbContext<CDbContext>(o => o.UseSqlite(dataSource));
            break;
         default:
            throw new Exception($"appsettings.json UseDatabase {useDatabase} not available");
      }
      return serviceCollection;
   } 

}