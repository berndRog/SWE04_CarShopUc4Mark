using CarShop.DomainModel.Entities;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CarShopTest.Application")]

namespace CarShop.Application.UseCasesUser;

internal class CAppCar : IAppCar {

   #region fields
   private IAppCore _appCore;
   private readonly IUnitOfWork _unitOfWork;
   private ILogger<CAppCar> _logger;
   #endregion

   #region ctor
   public CAppCar(
      IAppCore appCore,
      IUnitOfWork unitOfWork,
      ILogger<CAppCar> logger
   ){
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      _appCore = appCore;
      _unitOfWork = unitOfWork;   
   }
   #endregion

   #region methods
   public Result<Car> Create(Guid id, string make, string model, double price, int firstReg, string? imagePath) {
      _logger.LogDebug("Create()");

      if (_appCore.LoggedInUser.Id == NullUser.Instance.Id)
         return new Error<Car>($"Fehhler in Create(): User ist nicht eingeloggt");

      Car car = new Car().Set(id, make, model, price, firstReg, imagePath, NullUser.Instance);
      return new Success<Car>(car);
   }

   public Result<Car> Add(User user, Car car) {
      _logger.LogDebug("Add()");

      if (_appCore.LoggedInUser.Id != user.Id)
         return new Error<Car>($"Fehhler in AddCar(): User ist nicht eingeloggt");   

      try{
         _unitOfWork.RepositoryUser.Attach(user);                  
         user.AddCar(car);  // DomainModel
         _unitOfWork.RepositoryCar.Add(car);
   //    _unitOfWork.RepositoryUser.Update(seed.User1);
         _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();
         return new Success<Car>(car);
      }
      catch(Exception ex) {
         return new Error<Car>($"Fehhler in AddCar(): {ex.Message}");
      }   
   }

   public Result<Car> Update(User user, Car car, double price, string? imagePath) {
      _logger.LogDebug("Update()");
      
      if (_appCore.LoggedInUser.Id != user.Id)
         return new Error<Car>($"Fehhler in UpdateCar(): User ist nicht eingeloggt");

      try {
         Car? updCar = user.OfferedCars.FirstOrDefault(car => car.Id == car.Id);
         if (car.UserId != user.Id || updCar == null)
            return new Error<Car>($"Fehler in UpdateCar(): Car wird nicht von User angeboten");

         _unitOfWork.RepositoryUser.Attach(user);
         _unitOfWork.RepositoryCar.Attach(updCar);
         
         user.UpdateCar(car.Id, price, imagePath); // DomainModel
         
         _unitOfWork.RepositoryCar.Update(updCar);
         var result = _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();

         if (result) return new Success<Car>(updCar);
         else return new Error<Car>($"Fehler in UpdateCar()");
      }
      catch(Exception e) {
         return new Error<Car>($"Fehler in UpdateCar(): {e.Message}");
      }
   }

   public Result<Car> Remove(User user, Car car) {
      _logger.LogDebug("Remove()");

      if (_appCore.LoggedInUser.Id == NullUser.Instance.Id)
         return new Error<Car>($"Fehler in RemoveCar(): User ist nicht eingeloggt");

      try {
         user.RemoveCar(car); // DomainModel

         _unitOfWork.RepositoryCar.Remove(car);
         var result = _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();

         if (result) return new Success<Car>(car);
         else return new Error<Car>($"Fehler in RemoveCar()");
      }
      catch (Exception e) {
         return new Error<Car>($"Fehler in RemoveCar(): {e.Message}");
      }
   }
   #endregion
}