using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Utilities;

using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("CarShopTest.Application")]

namespace CarShop.Application.UseCaseCore;

internal class CAppCore : IAppCore {
   #region fields
   private User _user = NullUser.Instance;
   private readonly IUnitOfWork _unitOfWork;
   private readonly ILogger<CAppCore> _logger;
   #endregion

   #region properties IAppCore
   public User LoggedInUser{
      get => _user;
      set{
         if(value == null || value.Equals(_user)) return;
         _user = value;
      }
   }
   #endregion

   #region ctor
   public CAppCore(
      IUnitOfWork unitOfWork,
      ILogger<CAppCore> logger
   ){ 
      _unitOfWork = unitOfWork; 
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
   }
   #endregion

   #region methods
   public void Init() {
      _unitOfWork.Init();
   }
   #endregion

   #region methods - Login, Logout
   public Result<User> Login(string userName, string password){
      // is user registred
      var user = _unitOfWork.RepositoryUser.Find(user => user.UserName == userName);
      _unitOfWork.Dispose();
      if(user == null)
         return new Error<User>($"Nutzer {userName} gibt es nicht");

      // Compare passwords
      else if(user != null){
         var salt = user.Salt;
         var hashed = user.Password;

         if(AppSecurity.Compare(password, hashed, salt)){
            LoggedInUser = user;
            LoggedInUser.Role = Role.Customer;
            return new Success<User>(LoggedInUser);
         }
      }
      return new Error<User>($"Login fehlgeschlagen");
   }

   public Result<User> Logout(){
      LoggedInUser = NullUser.Instance;
      return new Success<User>(LoggedInUser);
   }
   #endregion

   #region methods Cars
   //public int CountCars(string make = "",string model = "",CarTs? carTs = null) {
   //   return _unitOfWork.RepositoryCar.CountCars(make, model, carTs);
   //}
   //public IEnumerable<string> GetMakesOrModels(string make = "") {
   //   return _unitOfWork.RepositoryCar.SelectMakesOrModels(make);
   //}
   #endregion
}