using Microsoft.Extensions.Logging;
using CarShop.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
[assembly: InternalsVisibleToAttribute("CarShopTest.Persistence")]

namespace CarShop.Persistence.Repositories; 
internal class CRepositoryUser: IRepositoryUser {

   #region fields
   private readonly CDbContext _dbContext;
   private readonly ILogger<CRepositoryUser> _logger;
   #endregion
   
   #region ctor
   public CRepositoryUser(
      CDbContext dbContext,
      ILoggerFactory loggerFactory
   ) {   
      _dbContext = dbContext;
      _logger = loggerFactory.CreateLogger<CRepositoryUser>();
      _logger.LogInformation("Ctor() {hashCode}", GetHashCode());
   }
   #endregion
     
   #region methods
   public User? FindById(
      Guid id, 
      bool address = false,
      bool offeredCars = false,
      bool markedCars = false
   ) {

      IQueryable<User> query = _dbContext.Users.AsQueryable();
      if (address) query = query.Include(u => u.Address);
      if (offeredCars) query = query.Include(u => u.OfferedCars);
      if (markedCars) {
         query = query.Include(u => u.MarkedCars)
                        .ThenInclude(m => m.Car);                           

      // query = query.ThenInclude(MarkedCars => MarkedCars.Car);
      }
      var result =  query.FirstOrDefault(u => u.Id == id);
      return result;
   }       

   public User? Find(
      Func<User,bool> predicate, 
      bool address = false,
      bool offeredCars = false
   ) {
      IQueryable<User> query = _dbContext.Users.AsQueryable();
      if (address) query = query.Include(u => u.Address);
      if (offeredCars) query = query.Include(u => u.OfferedCars);
      return query.FirstOrDefault(predicate);
   }

   public IEnumerable<User> Select(
      Func<User, bool> predicate, 
      bool address = false,
      bool offeredCars = false
   ) {
      IQueryable<User> query = _dbContext.Users.AsNoTracking();
      if (address) query = query.Include(u => u.Address);
      if (offeredCars) query = query.Include(u => u.OfferedCars);
      return query?.AsEnumerable().Where(predicate).ToList() ?? new ();
   }

   public IEnumerable<User> SelectAll(
      bool address = false,
      bool offerdCars = false
   ) {
      IQueryable<User> query = _dbContext.Users.AsNoTracking();
      if (address) query = query.Include(u => u.Address);
      if (offerdCars) query = query.Include(u => u.OfferedCars);
      return query.ToList() ?? new();
   }

   public void Add(User user) => 
      _dbContext.Users.Add(user);

   public void AddRange(IEnumerable<User> users) => 
      _dbContext.Users.AddRange(users);

   public void Update(User user) =>
      _dbContext.Users.Update(user);

   public void Remove(User user) => 
      _dbContext.Users.Remove(user);      

   public int Count() =>
      _dbContext.Users.Count();

   public void Attach(User user) =>
      _dbContext.Attach<User>(user);
    
   #endregion
}