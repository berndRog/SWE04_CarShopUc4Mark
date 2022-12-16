using CarShop.DomainModel.Entities;
using CarShop.DomainModel.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CarShopTest.Application")]
namespace CarShop.Application.UseCaseSearch; 
internal class CAppSearch: IAppSearch {
   #region fields
   private IAppCore _appCore;
   private readonly IUnitOfWork _unitOfWork;
   private ILogger<CAppSearch> _logger;
   #endregion

   #region ctor
   public CAppSearch(
      IAppCore appCore,
      IUnitOfWork unitOfWork,
      ILogger<CAppSearch> logger
   ){
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      _appCore = appCore;
      _unitOfWork = unitOfWork;   
   }
   #endregion

   #region methods
   public Result<IEnumerable<Car>> SearchCars(CarTs carTs) {
      _logger.LogDebug("SearchCars()");

      try{
         IEnumerable<Car> foundCars = _unitOfWork.RepositoryCar.SelectSearchedCars(carTs);
         return new Success<IEnumerable<Car>>(foundCars);
      }
      catch(Exception e){
         return new Error<IEnumerable<Car>>($"Fehler in SearchCars(): {e.Message}");
      }
   }

   public Result<MarkedCar> MarkCar(User user, Car car){
      _logger.LogDebug("Mark()");

      try{
         if(_appCore.LoggedInUser.Id != user.Id)
            return new Error<MarkedCar>($"Fehler in MarkCar(): User ist nicht eingeloggt");

         // DomainModel
         MarkedCar markedCar = user.AddMarkedCar(car);
         return new Success<MarkedCar>(markedCar);
      }
      catch(Exception e){
         return new Error<MarkedCar>($"Fehler in MarkCar(): {e.Message}");
      }
   }
   #endregion
}