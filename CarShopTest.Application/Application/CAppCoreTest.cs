using CarShop;
using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace CarShopTest.Application;
// Avoid parallel excution of tests with the same database
[Collection(nameof(SystemTestCollectionDefinition))]
public class CAppCoreTest {

   readonly Guid id = new Guid("00010000-0000-0000-0000-000000000000");
   const string firstName = "Martin";
   const string lastName = "Michel";
   const string email = "m.michel@gmx.de";
   const string phone = "05331 / 1234 9876";
   const string? imagePath = @"C:\CarShop\Images\image01.jpg";
   const string userName = "MartinM";
   const string password = "geh1m_";
   const Role role = Role.User;

   const string street = "Herbert-Meyer-Str.";
   const string number = "7";
   const string zip = "29556";
   const string city = "Suderburg";

   private readonly IAppCore _appCore;
   private readonly IAppUser _appUser;

   public CAppCoreTest() {
      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddApplicationTest();
      ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build ServiceProvider");

      CDbContext dbContext = serviceProvider.GetService<CDbContext>()
         ?? throw new Exception("Failed to build CDbContext");
      dbContext.Database.EnsureDeleted();
      dbContext.Database.EnsureCreated();

      _appCore = serviceProvider.GetService<IAppCore>()
         ?? throw new Exception("Failed to build CAppCore");

      _appUser = serviceProvider.GetService<IAppUser>()
         ?? throw new Exception("Failed to build CAppUser");
   }

   [Fact]
   public void LoginTest() {
      // Arrange
      Result<User> result = _appUser.Create(id, firstName, lastName, email, phone, imagePath, 
         userName, password, Role.Customer);
      result.Data.Should().NotBeNull();
      User user = result.Data! as User;
      _appUser.Register(user);
      // Act
      result = _appCore.Login(userName, password);
      // Assert
      result.Should().NotBeNull();
      result.Should().BeOfType<Success<User>>();
      var actual = result.Data! as User;
      actual.Should().NotBeNull();
      user.Role = Role.Customer;
      actual.Should().BeEquivalentTo(user);
   }

   [Fact]
   public void LogOutTest() {
      // Arrange
      Result<User> result = _appUser.Create(id, firstName, lastName, email, phone, imagePath, 
         userName, password, Role.Customer);
      result.Data.Should().NotBeNull();
      User user = result.Data! as User;
      _appUser.Register(user);
      _appCore.Login(userName, password);
      // Act
      result = _appCore.Logout();
      // Assert
      result.Should().NotBeNull();
      result.Should().BeOfType<Success<User>>();
      var actual = result.Data! as User;
      actual.Should().BeOfType<NullUser>();
   }
}