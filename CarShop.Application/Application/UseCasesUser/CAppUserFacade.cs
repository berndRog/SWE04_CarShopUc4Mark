using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CarShopTest.Application")]

namespace CarShop.Application.UseCasesUser;

internal class CAppUserFacade : IAppUser {

   #region fields
   private CUserRegister _userRegister;
   private CUserUpdate _userUpdate;
   private CUserRemove _userRemove;
   private IAppCore _appCore;
   private ILogger<CAppCar> _logger;
   #endregion

   #region ctor
   public CAppUserFacade(
      CUserRegister userRegister,
      CUserUpdate userUpdate,
      CUserRemove userRemove,
      IAppCore appCore,
      ILogger<CAppCar> logger
   ){
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      // Composition
      _userRegister = userRegister;
      _userUpdate = userUpdate;
      _userRemove = userRemove;
      _appCore = appCore;
   }
   #endregion

   #region methods interface IAppUser
   public Result<User> Create(Guid id, string firstName, string lastName, string email, string phone,
      string? imagePath, string userName, string password, Role role) =>
      _userRegister.Create(id, firstName, lastName, email, phone, imagePath, userName, password, role);

   public Result<User> Register(User user){
      if(_appCore.LoggedInUser == NullUser.Instance)
         return _userRegister.Register(user);
      else
         return new Error<User>("Nutzer ist eingeloggt. Register steht nicht zur Verfügung");
   }

   public Result<User> Update(User user, string? firstName, string? lastName, string? email, string? phone, 
      string? imagePath, string? password, string? street, string? number, string? zip, string? city) {
      if(_appCore.LoggedInUser != NullUser.Instance)
         return _userUpdate.Update(user, firstName, lastName, email, phone, imagePath, password, 
            street, number, zip, city );
      else
         return new Error<User>("Nutzer ist nicht eingeloggt. Update steht nicht zur Verfügung");
   }

   public Result<User> Remove(User user){
      if(_appCore.LoggedInUser.Id == user.Id)
         return _userRemove.Remove(user);
      else
         return new Error<User>("Nutzer ist nicht eingeloggt. Remove steht nicht zur Verfügung");
   }
   #endregion

}