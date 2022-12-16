using CarShop.Persistence.Repositories;
using CarShop.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
[assembly: InternalsVisibleToAttribute("CarShopTest.Persistence")]

namespace CarShop.Persistence;
internal class CUnitOfWork : IUnitOfWork {

   #region fields
   private readonly IServiceScopeFactory _serviceScopeFactory;
   private readonly IConfiguration _configuration;
   private readonly ILoggerFactory _loggerFactory;
   private readonly ILogger<CUnitOfWork> _logger;
   private CDbContext? _dbContext = null;
   private IRepositoryUser _repositoryUser = default!;
   private IRepositoryAddress _repositoryAddress = default!;
   private IRepositoryCar _repositoryCar = default!;
   private IRepositoryMarkedCar _repositoryMarkedCar = default!;
   #endregion

   #region properties    (simulation property injection) 
   public IRepositoryUser RepositoryUser {
      get {
         PropertyInjection();
         return _repositoryUser;
      }
   }
   public IRepositoryAddress RepositoryAddress {
      get {
         PropertyInjection();
         return _repositoryAddress;
      }
   }
   public IRepositoryCar RepositoryCar {
      get {
         PropertyInjection();
         return _repositoryCar;
      }
   }
   public IRepositoryMarkedCar RepositoryMarkedCar {
      get {
         PropertyInjection();
         return _repositoryMarkedCar;
      }
   }
   #endregion

   #region ctor
   public CUnitOfWork(
      IServiceScopeFactory serviceScopeFactory,
      IConfiguration configuration,
      ILoggerFactory loggerFactory
   ) {
      _serviceScopeFactory = serviceScopeFactory;
      _configuration = configuration;
      _loggerFactory = loggerFactory;
      _logger = loggerFactory.CreateLogger<CUnitOfWork>();
      _logger.LogInformation("Ctor() {hashCode}", GetHashCode());
   }
   #endregion

   #region methods
   public void Init() {
      _logger.LogDebug("Init()");
      var configSection = _configuration.GetSection("InitDatabase");
      var bClearDbTables = configSection.GetSection("ClearDbTables").Value?.ToLower() == "true";
      var bSeedDbTables = configSection.GetSection("SeedDbTables").Value?.ToLower() == "true";

      if (bClearDbTables) {
         var users = RepositoryUser.SelectAll();
         _dbContext?.Users.RemoveRange(users);
         SaveAllChanges();
      }
      if (bSeedDbTables) {
         var seed = new CSeed();
         RepositoryUser.AddRange(seed.Users);
         SaveAllChanges();

         seed.InitAddresses();
         foreach(var user in seed.Users ) {
            RepositoryUser.Update(user);
         }
         SaveAllChanges();
         
         seed.InitCars();
         foreach(var user in seed.Users ) {
            RepositoryUser.Update(user);
         }
         SaveAllChanges();

      }
      if (bClearDbTables || bSeedDbTables) Dispose();
   }

   // simulating property injection
   private void PropertyInjection() {
      if (_dbContext != null) return;
      _logger.LogDebug("PropertyInjection() {hashCode}", GetHashCode());
      IServiceScope scope = _serviceScopeFactory.CreateScope()
                        ?? throw new ApplicationException("Cannot create Scope");
      _dbContext = scope.ServiceProvider.GetService<CDbContext>()
                        ?? throw new ApplicationException("Cannot create CDbContext");
      _repositoryUser = new CRepositoryUser(_dbContext, _loggerFactory);
      _repositoryAddress = new CRepositoryAddress(_dbContext, _loggerFactory);
      _repositoryCar = new CRepositoryCar(_dbContext, _loggerFactory);
      _repositoryMarkedCar = new CRepositoryMarkedCar(_dbContext, _loggerFactory);
   }

   public void Dispose() {
      _logger.LogDebug("Dispose() {hasCode}", GetHashCode());
      _logger.LogDebug("\n{tracker}", _dbContext?.ChangeTracker.DebugView.LongView);
      _dbContext?.Dispose();
      _dbContext = null;
      _repositoryUser = default!;
      _repositoryAddress = default!;
      _repositoryCar = default!; 
      _repositoryMarkedCar = default!;
   }

   public bool SaveAllChanges() =>
      _dbContext?.SaveAllChanges() ?? false;

   public void LogChangeTracker(string text) {
      if (_dbContext != null) _dbContext.LogChangeTracker(text);
      else _logger.LogDebug("*** no DbContext availible *** {text}", text);
   }
   #endregion
}