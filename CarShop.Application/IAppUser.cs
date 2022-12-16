using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;

namespace CarShop; 
public interface IAppUser {
   Result<User> Create(
      Guid id, string firstName, string lastName, string email, string phone, 
      string? imagePath, string userName, string password, Role role);
   Result<User> Register(User   user);
   Result<User> Update(User user, string? firstName, string? lastName, string? email, 
      string? phone, string? imagePath, string? password, 
      string? street, string? number, string? zip, string? city);
   Result<User> Remove(User user);
}
