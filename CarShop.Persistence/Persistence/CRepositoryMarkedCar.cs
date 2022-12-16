using CarShop.DomainModel.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
[assembly: InternalsVisibleToAttribute("CarShopTest.Persistence")]

namespace CarShop.Persistence.Repositories; 
internal class CRepositoryMarkedCar: IRepositoryMarkedCar {

   #region fields
   private readonly CDbContext _dbContext;
   private readonly ILogger<CRepositoryMarkedCar> _logger;
   #endregion

   #region ctor
   public CRepositoryMarkedCar(
      CDbContext dbContext,
      ILoggerFactory loggerFactory
   ) {   
      _dbContext = dbContext;
      _logger = loggerFactory.CreateLogger<CRepositoryMarkedCar>();
      _logger.LogInformation("Ctor() {hashCode}", GetHashCode());
   }
   #endregion

   #region methods
   public MarkedCar? FindById(Guid UserId, Guid CarId) {
      IQueryable<MarkedCar> query = _dbContext.MarkedCars.AsQueryable();
      query = query.Include(m => m.User);
      query = query.Include(m => m.Car);
      return query.FirstOrDefault(m => m.UserId == UserId && m.CarId == CarId);
   }
                                              
   public IEnumerable<MarkedCar> SelectByUserId(Guid UserId) {
      IQueryable<MarkedCar> query = _dbContext.MarkedCars.AsQueryable();
      query = query.Include(m => m.User);
      query = query.Include(m => m.Car);
      return query.Where(m => m.UserId == UserId)
                  .ToList();
   }
   
   public IEnumerable<MarkedCar> SelectByCarId(Guid CarId) {
      IQueryable<MarkedCar> query = _dbContext.MarkedCars.AsQueryable();
      query = query.Include(m => m.User);
      query = query.Include(m => m.Car);
      return query.Where(m => m.CarId == CarId)
                  .ToList();
   }

   public void Add(MarkedCar markedCar) => 
      _dbContext.MarkedCars.Add(markedCar);

   public void Update(MarkedCar markedCar) =>
      _dbContext.MarkedCars.Add(markedCar);

   public void Remove(MarkedCar markedCar) =>
      _dbContext.MarkedCars.Remove(markedCar);

   public void Attach(MarkedCar markedCar) =>
      _dbContext.MarkedCars.Attach(markedCar);
   #endregion
}