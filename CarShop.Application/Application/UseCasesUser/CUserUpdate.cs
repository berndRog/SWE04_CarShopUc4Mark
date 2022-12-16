using CarShop.DomainModel.Entities;
using CarShop.Utilities;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("CarShopTest.Application")]

namespace CarShop.Application.UseCasesUser;
internal class CUserUpdate {

   #region fields
   private readonly IUnitOfWork _unitOfWork;
   private readonly ILogger<CUserUpdate> _logger;
   #endregion

   #region ctor
   public CUserUpdate(
      IUnitOfWork unitOfWork,
      ILogger<CUserUpdate> logger
   ) {
      _logger = logger;
      _logger.LogDebug("Ctor() {hashCode}", GetHashCode());
      _unitOfWork = unitOfWork;
   }
   #endregion

   #region methods
   public Result<User> Update(User user, string? firstName, string? lastName, string? email, string? phone, string? imagePath, 
      string? password, string? street, string? number, string? zip, string? city
   ) {
      _logger.LogDebug("Update()");

      try {
         // Find user
         var updUser = _unitOfWork.RepositoryUser.FindById(user.Id, true);
         if(updUser == null)
            return new Error<User>($"Fehler in UpdateUser()");

         // update with input values
         if(!string.IsNullOrEmpty(firstName)) updUser.FirstName = firstName;
         if(!string.IsNullOrEmpty(lastName)) updUser.LastName = lastName;
         if(!string.IsNullOrEmpty(email)) updUser.Email = email;
         if(!string.IsNullOrEmpty(phone)) updUser.Phone = phone;
         updUser.ImagePath = imagePath;
         if(!string.IsNullOrEmpty(password)){
            var (salt, hashed) = AppSecurity.HashPassword(password);
            updUser.Password = hashed;
            updUser.Salt = salt;
         }

         // Add or update Address
         if(street != null && number != null && zip != null && city != null){
            if(updUser.Address == null){
               // Add address
               updUser.AddOrUpdateAddress(street, number, zip, city);
               _unitOfWork.RepositoryAddress.Add(updUser.Address!); // not necessary
            }
            else{
               // update address
               updUser.AddOrUpdateAddress(street, number, zip, city);
               _unitOfWork.RepositoryAddress.Update(updUser.Address!); // not necessary
            }
         }

         _unitOfWork.RepositoryUser.Update(updUser!);
         var result = _unitOfWork.SaveAllChanges();
         _unitOfWork.Dispose();
         if(result) return new Success<User>(updUser!);
         else return new Error<User>($"Fehler in UpdateUser()");
      }
      catch(Exception e){
         return new Error<User>($"Fehler in UpdateUser(): {e.Message}");
      }
   }
   #endregion
}