using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Utilities;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]
namespace CarShop.Application.UseCasesUser;
internal class CUserRegister {

   #region fields
   private readonly IUnitOfWork _unitOfWork;
   private readonly ILogger<CUserRegister> _logger;
   #endregion

   #region ctor
   public CUserRegister(
      IUnitOfWork unitOfWork,
      ILogger<CUserRegister> logger
   ) {
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      _unitOfWork = unitOfWork;
   }
   #endregion

   #region methods - Create, Register
   public Result<User> Create(
      Guid id, string firstName, string lastName, string email, string phone,
      string? imagePath, string userName, string password, Role role
   ) {
      _logger.LogDebug("Create User");
      try {
         var result = _unitOfWork.RepositoryUser
                                 .Find(user => user.UserName == userName);
         _unitOfWork.Dispose();
         if (result != null) return new Error<User>($"UserName {userName} gibt es bereits.");
         // Hash password
         var (salt, hashed) = AppSecurity.HashPassword(password);
         // Create new User
         var user = new User().Set(id, firstName, lastName, email, phone, imagePath, 
            userName, hashed, salt, Role.User);
         return new Success<User>(user);
      }
      catch(Exception e){
         return new Error<User>($"Fehler in Create User(): {e.Message}");
      }

   }

   public Result<User> Register(User user) {
      _logger.LogDebug("Register User");

      try {
         // Insert user into repository
         _unitOfWork.RepositoryUser.Add(user);
         var result = _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();

         if(result) return new Success<User>(user);
         else return new Error<User>($"Fehler in Register User: Insert {user.FirstName} {user.LastName}");
      }
      catch(Exception e){
         return new Error<User>($"Fehler in Register User: {e.Message}");
      }
   }
   #endregion
}