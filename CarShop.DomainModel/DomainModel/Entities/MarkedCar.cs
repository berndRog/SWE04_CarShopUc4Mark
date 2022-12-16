namespace CarShop.DomainModel.Entities; 

public class MarkedCar {
   // Many-to-Many         MarkedCar ---> User [1]     
   //               [0..*] MarkedCar <-- User    
   public User User   { get; set; } = NullUser.Instance!;
   public Guid UserId { get; set; } = Guid.Empty;
   //                      MarkedCar ---> Car [1]
   //               [0..*] MarkedCar <--- Car    
   public Car Car    { get; set; } = NullCar.Instance;
   public Guid CarId { get; set; } = Guid.Empty;

   public MarkedCar() { }

   public MarkedCar Set(User user, Car car) { 
      User = user;
      UserId = user.Id;

      Car = car;
      CarId = car.Id;
      return this;
   }
}