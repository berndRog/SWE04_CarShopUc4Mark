// using CarShop.DomainModel.Entities;
// using CarShop.Utilities;
//
// namespace CarShopTest.DomainModel.Entities;
// public class MarkedCarUt {
//
//    private readonly Guid _id = new Guid("f0000001-0000-0000-0000-000000000000");
//
//    [Fact]
//    public void CtorUt() {
//       // Arrange
//       // Act
//       var actual = new MarkedCar();
//       // Assert
//       actual.Should().NotBeNull() .And. BeOfType<MarkedCar>();
//    }
//
//    [Fact]
//    public void CtorSetUt() {
//       // Arrange
//       CSeed seed = new CSeed();
//       User user = seed.DefaultUser;
//       Car car = seed.DefaultCar;
//       // Act
//       var actual = new MarkedCar().Set(_id,user,car);
//       // Assert
//       actual.Should().NotBeNull() .And. BeOfType<MarkedCar>();
//       actual.Id.Should().Be(_id);
//       actual.User.Should().BeEquivalentTo(user);
//       actual.UserId.Should().Be(user.Id);
//       actual.Car.Should().BeEquivalentTo(car);
//       actual.CarId.Should().Be(car.Id);
//    }
//
//    [Fact]
//    public void ObjectInitializerUt() {
//       // Arrange
//       CSeed seed = new CSeed();
//       User user = seed.DefaultUser;
//       Car car = seed.DefaultCar;
//       // Act
//       var actual = new MarkedCar{
//          Id = Guid.NewGuid(),
//          User = user,
//          UserId = user.Id,
//          Car = car,
//          CarId = car.Id,
//       };
//    }
//
//    [Fact]
//    public void AddCarToMarkedCars1ListUt() {
//       // Arrange
//       CSeed seed = new CSeed().InitAddresses().InitCars();
//       User user1 = seed.User1;
//       // Act
//       user1.AddCarToMarkedCarsList(seed.Car01);
//       user1.AddCarToMarkedCarsList(seed.Car02);
//       // Assert
//       user1.MarkedCars.Should().HaveCount(2);
//    }
//
//       [Fact]
//    public void AddCarToMarkedCars2ListUt() {
//       // Arrange
//       CSeed seed = new CSeed().InitAddresses().InitCars();
//       User user1 = seed.User1;
//       User user2 = seed.User2;
//       // Act
//       user1.AddCarToMarkedCarsList(seed.Car01);
//       user1.AddCarToMarkedCarsList(seed.Car02);
//       user2.AddCarToMarkedCarsList(seed.Car02);
//       user2.AddCarToMarkedCarsList(seed.Car03);
//       // Assert
//       user1.MarkedCars.Should().HaveCount(2);
//    }
//
// }
