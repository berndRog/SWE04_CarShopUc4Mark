using CarShop.DomainModel.Entities;
using CarShop.DomainModel.ValueObjects;

namespace CarShop; 
public interface IRepositoryCar {
   Car? FindById(Guid id);
   Car? Find(Func<Car, bool> predicate);
   IEnumerable<Car> Select(Func<Car, bool> predicate);
   IEnumerable<Car> SelectAll();
   void Add(Car car);
   void AddRange(IEnumerable<Car> cars);
   void Update(Car car);
   void Remove(Car car);
   void Attach(Car car);
   int Count();
   int Count(string make);
   int Count(string make, string model);
   int Count(CarTs carTs);
   IEnumerable<string> SelectMakes();
   IEnumerable<string> SelectModels(string make = "");
   IEnumerable<Car> SelectSearchedCars(CarTs carTs);
}
