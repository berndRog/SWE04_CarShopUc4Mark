using CarShop;
using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Persistence;
using CarShop.Utilities;

using Microsoft.Extensions.DependencyInjection;

namespace CarShopTest.Application;
// Avoid parallel excution of tests with the same database
[Collection(nameof(SystemTestCollectionDefinition))]
public class CAppOfferCarTest {

   readonly Guid id = new Guid("c0010000-0000-0000-0000-000000000000");
   const string make = "Ford";
   const string model = "Fiesta";
   const double price = 1234.0;
   const int firstReg = 2011;
   const string? imagePath = @"C:\CarShop\images\car01.jpg";

   private readonly CSeed _seed ;
   private IAppCore _appCore;
   private IAppUser _appUser;
   private IAppCar _appOfferCar;
   private readonly IUnitOfWork _unitOfWork;

   public CAppOfferCarTest() {

      _seed = new CSeed();

      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddApplicationTest();
      ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build ServiceProvider");

      CDbContext dbContext = serviceProvider.GetService<CDbContext>()
         ?? throw new Exception("Failed to build CDbContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();

      _unitOfWork = serviceProvider.GetService<IUnitOfWork>()
         ?? throw new Exception("Failed to build IUnitOfWork");

      _appCore = serviceProvider.GetService<IAppCore>()
         ?? throw new Exception("Failed to build IAppCore");

      _appUser = serviceProvider.GetService<IAppUser>()
         ?? throw new Exception("Failed to build IAppUser");

      _appOfferCar = serviceProvider.GetService<IAppCar>()
         ?? throw new Exception("Failed to build IAppUser");
   }

   [Fact]
   public void CreateTest() {
      // Arrange
      string userName = _seed.User1.UserName;
      string password = "geh1m_";
      _appUser.Register(_seed.User1);
      _appCore.Login(userName, password);

      // Act
      var result = _appOfferCar.Create(id, make, model, price, firstReg, imagePath);
      // Assert
      result.Should().NotBeNull();
      result.Should().BeOfType<Success<Car>>();
      result.Data.Should().NotBeNull();
      Car actual = result.Data! as Car;
      result.Data.Should().BeOfType<Car>();
      actual.Id.Should().Be(id);
      actual.Make.Should().Be(make);
      actual.Model.Should().Be(model);
      actual.Price.Should().Be(price);
      actual.FirstReg.Should().Be(firstReg);
      actual.ImagePath.Should().Be(imagePath);
   }

   [Fact]
   public void AddTest() {
      // Arrange
      string userName = _seed.User1.UserName;
      string password = "geh1m_";
      _appUser.Register(_seed.User1);
      _appCore.Login(userName, password);
      Result<Car> result = _appOfferCar.Create(id, make, model, price, firstReg, imagePath);
      Car car = result!.Data!;
      // Act
      result = _appOfferCar.Add(_seed.User1, car);
      // Assert
      result.Should().BeOfType<Success<Car>>();
      Car expected = result.Data!;
      var actual = _unitOfWork.RepositoryCar.FindById(car.Id);
      _unitOfWork.Dispose();
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void UpdateTest() {
      // Arrange
      string userName = _seed.User1.UserName;
      string password = "geh1m_";
      _appUser.Register(_seed.User1);
      _appCore.Login(userName, password);
      Result<Car> result = _appOfferCar.Create(id, make, model, price, firstReg, imagePath);
      Car car = result!.Data!; 
      _appOfferCar.Add(_seed.User1, car);
      // Act
      double  updPrice = 999;
      string? updImagePath = @"D:\CarShop\NewImages\Car01.png";
      result = _appOfferCar.Update(_seed.User1, car, updPrice, updImagePath);
      // Assert
      result.Should().BeOfType<Success<Car>>();
      Car expected = result.Data!;
      var actual = _unitOfWork.RepositoryCar.FindById(car.Id);
      _unitOfWork.Dispose();      
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void RemoveTest() {
      // Arrange
      string userName = _seed.User1.UserName;
      string password = "geh1m_";
      _appUser.Register(_seed.User1);
      _appCore.Login(userName, password);
      Result<Car> result = _appOfferCar.Create(id, make, model, price, firstReg, imagePath);
      Car car = result!.Data!;
      result = _appOfferCar.Add(_seed.User1, car);
      // Act
      result = _appOfferCar.Remove(_seed.User1, car);
      // Assert
      result.Should().BeOfType<Success<Car>>();
      Car expected = result.Data!;
      var actual = _unitOfWork.RepositoryCar.FindById(car.Id);
      _unitOfWork.Dispose();
      actual.Should().BeNull();
   }
}