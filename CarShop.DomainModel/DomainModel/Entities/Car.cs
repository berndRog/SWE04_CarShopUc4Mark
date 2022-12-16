using CarShop.Utilities;
namespace CarShop.DomainModel.Entities;

public class Car: ABaseEntity {

   #region properties
   public override Guid Id { get; set; } = Guid.Empty;
   public string Make { get; set; } = string.Empty;
   public string Model { get; set; } = string.Empty;
   public double Price { get; set; } = 0.0;
   public int FirstReg { get; set; } = 0;
   public string? ImagePath { get; set; } = null;

   // Inverse Navigation-Property one-to-many
   // Car --> User [0..1]
   public User User { get; set; } = NullUser.Instance;
   public Guid UserId { get; set; } = Guid.Empty;

   // Navigation-Property User --> MarkedCars [0..*] 
   public IList<MarkedCar> MarkedCars { get; set; } = new List<MarkedCar>();

   #endregion

   #region ctor
   public Car() { Id = Guid.NewGuid();}
   #endregion

   #region methods
   public Car Set(Guid id, string make, string model, double price, int firstReg, 
      string? imagePath, User user) {
      Id = (id == Guid.Empty) ? Guid.NewGuid() : id; 
      Make = make;
      Model = model;
      Price = price;
      FirstReg = firstReg;
      ImagePath = imagePath;
      User = user;
      return this;
   }
   public string AsString() =>
      $"{Id.ToString()[0..8]} {Make} {Model} {Price,8:f2} Reg:{FirstReg} "+
      $"User {User.LastName} {UserId.ToString()[0..8]} ";
   #endregion  
}