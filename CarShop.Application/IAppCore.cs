using CarShop.DomainModel.Entities;
namespace CarShop; 
public interface IAppCore {
   public void Init();
   public User  LoggedInUser {get; set; }
   Result<User> Login(string userName, string password);
   Result<User> Logout();
}