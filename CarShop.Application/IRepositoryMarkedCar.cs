using CarShop.DomainModel.Entities;
namespace CarShop;

public interface IRepositoryMarkedCar {
   MarkedCar? FindById(Guid UserId, Guid CarId);
   IEnumerable<MarkedCar> SelectByUserId(Guid UserId);
   IEnumerable<MarkedCar> SelectByCarId(Guid CarId);
   void Add(MarkedCar markedCar);
   void Update(MarkedCar markedCar);
   void Remove(MarkedCar markedCar);
   void Attach(MarkedCar markedCar);
}