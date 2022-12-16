using CarShop.DomainModel.Entities;
using System.Runtime.CompilerServices;
using CarShop.DomainModel.ValueObjects;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
[assembly: InternalsVisibleToAttribute("CarShopTest.Persistence")]

namespace CarShop.Persistence.Repositories; 
internal class CRepositoryCar: IRepositoryCar {

   #region fields
   private readonly CDbContext _dbContext;
   private readonly ILogger<CRepositoryCar> _logger;
   #endregion
   
   #region ctor
   public CRepositoryCar(
      CDbContext dbContext,
      ILoggerFactory loggerFactory
   ) {   
      _dbContext = dbContext;
      _logger = loggerFactory.CreateLogger<CRepositoryCar>();
      _logger.LogInformation("Ctor() {hashCode}", GetHashCode());
   }
   #endregion
     
   #region methods 
   // for joins we need to include 
   // using Microsoft.EntityFrameworkCore;

   public Car? FindById(Guid id) =>
      _dbContext.Cars.AsQueryable()
         .Include(c => c.User)
         .FirstOrDefault(c => c.Id == id);
   
   public Car? Find(Func<Car,bool> predicate) =>
      _dbContext.Cars.AsQueryable()
         .Include(c => c.User)
         .FirstOrDefault(predicate);
   
   public IEnumerable<Car> Select(Func<Car, bool> predicate) =>
      _dbContext.Cars.AsQueryable()
         .Include(c => c.User)
         .Where(predicate)
         .ToList();
                   
   public IEnumerable<Car> SelectAll()  =>
      _dbContext.Cars.AsQueryable()
         .Include(c => c.User)
         .ToList();

   public void Add(Car car) => 
      _dbContext.Cars.Add(car);

   public void AddRange(IEnumerable<Car> cars) => 
      _dbContext.Cars.AddRange(cars);

   public void Update(Car car) =>
      _dbContext.Cars.Update(car);

   public void Remove(Car car) => 
      _dbContext.Cars.Remove(car);      

   public void Attach(Car car) => 
      _dbContext.Cars.Attach(car);
   #endregion

   #region methods Count Make, Model, CarTs
   public int Count() =>
      _dbContext.Cars.Count();

   public int Count(string make) {
      if (string.IsNullOrWhiteSpace(make)) return _dbContext.Cars.Count();
      else  return _dbContext.Cars.Count(car => car.Make == make);
   }

   public int Count(string make, string model) {
      if(string.IsNullOrWhiteSpace(make) && string.IsNullOrWhiteSpace(model))
         return Count();
      else if(string.IsNullOrWhiteSpace(model))
         return Count(make);
      else
         return _dbContext.Cars.Count(car => car.Make == make && car.Model == model);
   }
      
   public int Count(CarTs carTs) =>          
      _dbContext.Cars.Count(c =>
         c.Make == carTs.Make &&
         c.Make == carTs.Make &&
         c.Model == carTs.Model &&
         c.Price >= carTs.PriceFrom &&
         c.Price <= carTs.PriceTo &&
         c.FirstReg >= carTs.RegFrom &&
         c.FirstReg <= carTs.RegTo);      
   #endregion

   #region methods Select Make, Model, CarTs
   public IEnumerable<string> SelectMakes() =>
      //  IQueryable<Car> q1 = _dbContext.Cars as IQueryable<Car>;
      //  IQueryable<string> q2 = q1.Select(car => car.Make);
      //  IQueryable<string> q3 = q2.Distinct();
      //  IOrderedQueryable<string> q4 = q3.OrderBy(make => make);
      //  List<string> result = q4.ToList() ?? new List<string>();
      //  _logger.LogDebug("SelectMakes: {Count}", result.Count);
      //  return result;
      _dbContext.Cars 
                .Select(car => car.Make)
                .Distinct()
                .OrderBy(make => make)
                .ToList() ?? new List<string>();

   public IEnumerable<string> SelectModels(string make){
      if(string.IsNullOrWhiteSpace(make))
         return new List<string>();
      else
         return _dbContext.Cars
                  .Where(car => car.Make == make)
                  .Select(car => car.Model)
                  .Distinct()
                  .OrderBy(model => model)
                  .ToList() ?? new List<string>();
   }

   public IEnumerable<Car> SelectSearchedCars(CarTs carTs) =>
      _dbContext.Cars
                .Where(c =>
                        c.Make == carTs.Make &&
                        c.Make == carTs.Make &&
                        c.Model == carTs.Model &&
                        c.Price >= carTs.PriceFrom &&
                        c.Price <= carTs.PriceTo &&
                        c.FirstReg >= carTs.RegFrom &&
                        c.FirstReg <= carTs.RegTo 
                     ).ToList() ?? new List<Car>();
   
   #endregion

}