using CarShop.DomainModel.Entities;
namespace CarShop;

public interface IRepositoryAddress {
   void Add(Address address);
   void Update(Address address);
   void Remove(Address address);
   void Attach(Address address);
}