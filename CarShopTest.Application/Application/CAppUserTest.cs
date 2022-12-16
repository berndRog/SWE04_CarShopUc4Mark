using CarShop;
using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Persistence;
using CarShop.Utilities;

using Microsoft.Extensions.DependencyInjection;

namespace CarShopTest.Application;
// Avoid parallel excution of tests with the same database
[Collection(nameof(SystemTestCollectionDefinition))]
public class CAppUserTest {

   readonly Guid id = new Guid("c0010000-0000-0000-0000-000000000000");
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
   private readonly IUnitOfWork _unitOfWork;

   public CAppUserTest() {
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
   }

   #region Test User Create, Register
   [Fact]
   public void CreateTest() {
      // Arrange
      // Act
      var result = _appUser.Create(id, firstName, lastName, email, phone, imagePath,
                                   userName, password, Role.Customer);
      // Assert
      result.Should().NotBeNull();
      result.Should().BeOfType<Success<User>>();
      result.Data.Should().NotBeNull();
      var actual = result.Data as User;
      result.Data.Should().BeOfType<User>();

      var hashed = AppSecurity.HashPbkdf2(password, actual!.Salt);
      actual.Id.Should().Be(id);
      actual.FirstName.Should().Be(firstName);
      actual.LastName.Should().Be(lastName);
      actual.Email.Should().Be(email);
      actual.Phone.Should().Be(phone);
      actual.ImagePath.Should().Be(imagePath);
      actual.UserName.Should().Be(userName);
      actual.Password.Should().Be(hashed);
      actual.Role.Should().Be(role);
   }

   [Fact]
   public void RegisterTest() {
      // Arrange
      Result<User> result = _appUser.Create(Guid.Empty, firstName, lastName, email, phone, imagePath,
         userName, password, Role.Customer);
      User user = result.Data! as User;
      // Act
      result = _appUser.Register(user);
      // Assert
      result.Should().BeOfType<Success<User>>();
      var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
      _unitOfWork.Dispose();
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(user, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void UpdateTest() {
      // Arrange
      Result<User> result = _appUser.Create(id, firstName, lastName, email, phone, imagePath,
         userName, password, Role.Customer);
      result.Should().BeOfType<Success<User>>();
      User user = result.Data! as User;
      _appUser.Register(user);
      Result<User> resultLogin = _appCore.Login(userName, password);
      result.Should().BeOfType<Success<User>>();
      string updFirstName = "Arne";
      string updLastName = "Arndt-Bayer";
      string updEmail = "a.arndt-byer@t-online.de";
      string updPhone = "0123 789 4567";
      string? updImagePath = null;
      string updPassword = "streng_geh1m_";
      // Act
      result = _appUser.Update(user, updFirstName, updLastName, updEmail, updPhone, updImagePath,
                               updPassword, null, null, null, null);
      // Assert
      result.Should().BeOfType<Success<User>>();
      User expected = result.Data! as User;
      var actual = _unitOfWork.RepositoryUser.FindById(user.Id, true);
      _unitOfWork.Dispose();
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
   }
   [Fact]
   public void UpdateUserAddAdressTest() {
      // Arrange
      Result<User> result = _appUser.Create(id, firstName, lastName, email, phone, imagePath,
         userName, password, Role.Customer);
      result.Should().BeOfType<Success<User>>();
      User user = result.Data! as User;
      _appUser.Register(user);
      result = _appCore.Login(userName, password);
      result.Should().BeOfType<Success<User>>();
      string updFirstName = "Arne";
      string updLastName = "Arndt-Bayer";
      string updEmail = "a.arndt-byer@t-online.de";
      string updPhone = "0123 789 4567";
      string? updImagePath = null;
      string updPassword = "streng_geh1m_";
      // Act
      result = _appUser.Update(user, updFirstName, updLastName, updEmail, updPhone, updImagePath,
                       updPassword, street, number, zip, city);
      // Assert
      result.Should().BeOfType<Success<User>>();
      User expected = result.Data! as User;
      var actual = _unitOfWork.RepositoryUser.FindById(user.Id, true);
      _unitOfWork.Dispose();
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void UpdateUserWithAddressTest() {
      // Arrange
      Result<User> result = _appUser.Create(id, firstName, lastName, email, phone, imagePath,
         userName, password, Role.Customer);
      result.Should().BeOfType<Success<User>>();
      User user = result.Data! as User;
      _appUser.Register(user);
      Result<User> resultLogin = _appCore.Login(userName, password);
      result.Should().BeOfType<Success<User>>();

      // Add address
      _appUser.Update(user, null, null, null, null, null, null,
         street, number, zip, city);

      string updFirstName = "Arne";
      string updLastName = "Arndt-Bayer";
      string updEmail = "a.arndt-byer@t-online.de";
      string updPhone = "0123 789 4567";
      string? updImagePath = null;
      string updPassword = "streng_geh1m_";
      string updStreet = "Bahnhofstr.";
      string updNumber = "1";
      string updZip = "29525";
      string updCity = "Uelzen";
      // Act update user and address
      result = _appUser.Update(user, updFirstName, updLastName, updEmail, updPhone, updImagePath,
                               updPassword, updStreet, updNumber, updZip, updCity);
      // Assert
      result.Should().BeOfType<Success<User>>();
      User expected = result.Data! as User;
      var actual = _unitOfWork.RepositoryUser.FindById(user.Id, true);
      _unitOfWork.Dispose();
      actual.Should().NotBeNull();
      actual.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
   }
   #endregion
}

