using CarShop.DomainModel.Enums;
using CarShop.Utilities;
namespace CarShop.DomainModel.Entities;

public class User: ABaseEntity {
   #region properties
   public override Guid Id{ get; set; } = Guid.Empty;
   public string FirstName{ get; set; } = string.Empty;
   public string LastName{ get; set; } = string.Empty;
   public string Email{ get; set; } = string.Empty;
   public string Phone{ get; set; } = string.Empty;
   public string? ImagePath{ get; set; } = null;
   public string UserName{ get; set; } = string.Empty;
   public string Password{ get; set; } = string.Empty;
   public string Salt{ get; set; } = string.Empty;
   public Role Role{ get; set; } = Role.User;
   // User --> Address [0..1]
   public Address? Address { get; set; } = null;
   // Navigation-Property User --> Cars [0..*]  
   public IList<Car> OfferedCars { get; set; } = new List<Car>();
   // Navigation-Property User --> MarkedCars [0..*] 
   public IList<MarkedCar> MarkedCars { get; set; } = new List<MarkedCar>();
   #endregion

   #region ctor
   public User(): base() {  Id = Guid.NewGuid();}
   #endregion

   #region methods
   public User Set(Guid id, string firstName, string lastName, 
      string email, string phone, string? imagePath,
      string username, string password, string salt, Role role) {
      Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      Phone = phone;
      ImagePath = imagePath;
      UserName = username;
      Password = password;
      Salt = salt;
      Role = role;
      return this;
   }
   public string AsString() {
      string role = Role switch {
         Role.User => "User",
         Role.Customer => "Customer",
         Role.Seller => "Seller",
         _ => "Unknown"
      };
      string s = $"{Id.ToString()[..8]} {FirstName} {LastName} {Email} {Phone} {ImagePath} " +
         $"{UserName} {role}";
      
      if(Address != null) s += Address.AsString();      
      return s;
   }  
   #endregion

   #region methods Address
   public void AddOrUpdateAddress(string street, string number, string zip, string city) {
      if (Address == null) {
         Address = new Address().Set(Guid.NewGuid(), street, number, zip, city);
         Address.User = this;
      }
      else {
         Address.Street = street;
         Address.Number = number;
         Address.Zip = zip;
         Address.City = city;
      }
   }

   public void AddOrUpdateAddress(Address address) {
      Address = address;
      Address.User = this;
   }
   #endregion

   #region methods OfferedCars
   public Car? FindCarById(Guid carId) =>
      OfferedCars.FirstOrDefault(car => car.Id == carId);
   
   public IList<Car> GetAllCars() =>
      OfferedCars.ToList();   

   public void AddCar(Car car) {
      car.User = this;
      car.UserId = this.Id;
      OfferedCars.Add(car);
   }

   public void UpdateCar(Guid carId, double price, string? imagePath) {
      Car? foundCar = OfferedCars.FirstOrDefault(c => c.Id == carId);
      if (foundCar == null)
         throw new Exception($"UpdateOfferedCars(): Car with id {Utils.Guid8(carId)} not found");
      foundCar.Price = price;
      foundCar.ImagePath = imagePath;
   }

   public void RemoveCar(Car car) {
      car.User = NullUser.Instance;
      car.UserId = NullUser.Instance.Id;
      OfferedCars.Remove(car);
   }

   public string? OfferedCarsAsString() {
      string? s = null;
      if(OfferedCars.Count > 0)      {
         s += "\n OfferedCars {";
         foreach (var car in OfferedCars) 
            s += $"{car.Id.ToString()[..9]} {car.Make} {car.Model}";
         s += "\n }";
      }
      return s;
   }
   #endregion

   #region methods MarkedCars
   public MarkedCar AddMarkedCar(Car car) {
      var markedCar = new MarkedCar {
         User = this,
         UserId = Id,
         Car = car,
         CarId = car.Id
      };
      MarkedCars.Add(markedCar);
      car.MarkedCars.Add(markedCar);
      return markedCar;
   }

   public void RemoveMarkedCar(Car car) {
      var markedCar = MarkedCars.FirstOrDefault(mc => mc.Car.Id == car.Id);
      if (markedCar != null) {
         MarkedCars.Remove(markedCar);
         car.MarkedCars.Remove(markedCar);
      }
   }
   #endregion


}