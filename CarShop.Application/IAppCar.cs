using CarShop.DomainModel.Entities;

namespace CarShop; 
public interface IAppCar {
   Result<Car> Create(
      Guid id, string make, string model, double price, int firstReg, string? imagePath);
   Result<Car> Add(User user, Car car);
   Result<Car> Update(User user, Car car, double price, string? imagePath);
   Result<Car> Remove(User user, Car car);
}
