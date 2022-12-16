using CarShop.DomainModel.Entities;

using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
namespace CarShop.Application.UseCasesUser;

internal class CUserRemove {

   #region fields
   private readonly IAppCore _appCore;
   private readonly IUnitOfWork _unitOfWork;
   private readonly ILogger<CUserUpdate> _logger;
   #endregion

   #region ctor
   public CUserRemove(
      IAppCore appCore,
      IUnitOfWork unitOfWork,
      ILogger<CUserUpdate> logger
   ) {
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      _unitOfWork = unitOfWork;
      _appCore = appCore;
   }
   #endregion
   
   #region methods 
   public Result<User> Remove(User user) {
      _logger.LogDebug("Remove()");
      try{
         if(_appCore.LoggedInUser == NullUser.Instance)
            return new Error<User>("Nutzer ist nicht eingeloggt. Update steht nicht zur Verfügung");
         // Delete user (cascading delete of address in database)
         _unitOfWork.RepositoryUser.Remove(user);
         var result = _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();
         if(result) return new Success<User>(user);
         else return new Error<User>($"Fehler in RemoveUser()");
      }
      catch(Exception e){
         return new Error<User>($"Fehler in RemoveUser: {e.Message}");
      }
   }
   #endregion
}