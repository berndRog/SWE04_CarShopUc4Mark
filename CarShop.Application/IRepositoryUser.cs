using CarShop.DomainModel.Entities;
namespace CarShop;

public interface IRepositoryUser {
   User? FindById(Guid id, bool address = false, bool car = false, bool markedCar = false);
   User? Find(Func<User, bool> predicate, bool address = false, bool car = false);
   IEnumerable<User> Select(Func<User, bool> predicate, bool address = false, bool car = false);
   IEnumerable<User> SelectAll(bool address = false, bool car = false);
   void Add(User user);
   void AddRange(IEnumerable<User> users);
   void Update(User user);
   void Remove(User user);
   int Count();
   void Attach(User user);
}