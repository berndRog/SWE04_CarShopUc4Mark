using CarShop.DomainModel.Entities;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
[assembly: InternalsVisibleToAttribute("CarShopTest.Persistence")]

namespace CarShop.Persistence.Repositories; 
internal class CRepositoryAddress: IRepositoryAddress {


   #region fields
   private readonly CDbContext _dbContext;
   private readonly ILogger<CRepositoryAddress> _logger;
   #endregion

   #region ctor
   public CRepositoryAddress(
      CDbContext dbContext,
      ILoggerFactory loggerFactory
   ) {   
      _dbContext = dbContext;
      _logger = loggerFactory.CreateLogger<CRepositoryAddress>();
      _logger.LogInformation("Ctor() {hashCode}", GetHashCode());
   }
   #endregion
     
   #region methods    
   public void Add(Address address) => 
      _dbContext.Addresses.Add(address);

   public void Update(Address address) =>
      _dbContext.Addresses.Update(address);

   public virtual void Remove(Address address) => 
      _dbContext.Addresses.Remove(address);
   
   public virtual void Attach(Address address) => 
      _dbContext.Addresses.Attach(address);   
   #endregion
}