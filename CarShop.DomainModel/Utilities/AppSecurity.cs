using System.Security.Cryptography;
namespace CarShop.Utilities; 

public static class AppSecurity {
   // https://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
   public static string GenerateSalt(int size) {
      var random = RandomNumberGenerator.Create();
      var bytes = new byte[size];
      random.GetBytes(bytes);
      return Convert.ToBase64String(bytes);
   }

   public static string HashPbkdf2(string pwd, string salt) {
      var saltBytes = Convert.FromBase64String(salt);
      var pbkdf2 = new Rfc2898DeriveBytes(pwd, saltBytes, 1000);
      var key = pbkdf2.GetBytes(64);
      return Convert.ToBase64String(key);
   }

   public static (string, string) HashPassword(string pw) {
      var salt = GenerateSalt(24);
      var pwHash = HashPbkdf2(pw, salt);
      return (salt, pwHash);
   }

   public static bool Compare(string password, string hashed, string salt) {
      var passHash = HashPbkdf2(password, salt);
      return passHash == hashed;
   }
}