using CarShop.DomainModel.Entities;
using CarShop.DomainModel.ValueObjects;
namespace CarShop; 
public interface IAppSearch {
   Result<IEnumerable<Car>> SearchCars(CarTs carTs);
   Result<MarkedCar> MarkCar(User user, Car car);
}