namespace CarShop.DomainModel.Entities; 

// https://jonskeet.uk/csharp/singleton.html
public class NullUser: User {  
   // Singleton Skeet Version 4
   private static readonly NullUser instance = new NullUser();
   
   public static NullUser Instance { get => instance; }

   static NullUser() { }
   private NullUser() { Id = Guid.Empty; }
}