using Microsoft.Extensions.Configuration;
using System.IO;
namespace CarShop.Persistence;

public static class CDbSelection {

   public static (string useDatabase, string dataSource) CreateDataSource(IConfiguration configuration) {

      var useDatabase= configuration.GetSection("UseDatabase").Value;
      var connectionString = configuration.GetSection("ConnectionStrings")[useDatabase];
      var directory= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

      switch(useDatabase){
         case "LocalDb":
            var dbFile = $"{Path.Combine(directory, connectionString)}.mdf";
            var dataSourceLocalDb = $"Data Source = (LocalDB)\\MSSQLLocalDB; " +
                                    $"Initial Catalog = {connectionString}; Integrated Security = True; " +
                                    $"AttachDbFileName = {dbFile};";
            return ( useDatabase, dataSourceLocalDb );
         case "SqlServer":
            return ( useDatabase, connectionString );
         case "Sqlite":
            var dataSourceSqlite =
               "Data Source=" + Path.Combine(directory, connectionString) + ".db";
            return ( useDatabase, dataSourceSqlite );
         default:
            throw new Exception($"appsettings.json UseDatabase {useDatabase} not available");
      }
   }
}