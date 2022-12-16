namespace CarShop.DomainModel.Entities; 

public  class NullCar: Car {  // Singleton
   private static readonly NullCar instance = new NullCar();
   public static NullCar Instance => instance; 
   static NullCar() {  }
   private NullCar() { Id = Guid.Empty; }
}