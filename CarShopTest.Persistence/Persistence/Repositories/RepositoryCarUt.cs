using CarShop;
using CarShop.DomainModel.Entities;
using CarShop.Persistence;
using CarShop.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace CarShopTest.Persistence.Repositories;

[Collection(nameof(SystemTestCollectionDefinition))]
public class RepositoryCarUt {

   private readonly IUnitOfWork _unitOfWork;

   public RepositoryCarUt() {
      IServiceCollection serviceCollection = new ServiceCollection();
      serviceCollection.AddPersistenceTest();
      ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider()
         ?? throw new Exception("Failed to build ServiceProvider");
      
      var unitOfWork = serviceProvider.GetService<IUnitOfWork>();
      unitOfWork.Should().NotBeNull();
      _unitOfWork = unitOfWork!;

      var dbContext = serviceProvider.GetService<CDbContext>();
      dbContext.Should().NotBeNull();
      dbContext!.Database.EnsureDeleted();
      dbContext!.Database.EnsureCreated();
      dbContext!.Dispose();
   }

   [Fact]
   public void FindbyIdWithRepoCarUt() {
      // Arrange
      var seed = new CSeed();
      seed.User1.AddCar(seed.Car01);
      _unitOfWork.RepositoryCar.Add(seed.Car01);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.FindById(seed.Car01.Id);
      // Assert (omit nested reference car.User)
      actual.Should().NotBeNull();
      seed.Car01.Should().BeEquivalentTo(actual, opt => opt.IgnoringCyclicReferences());
   }
   [Fact]
   public void FindByPredicateUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryCar.AddRange(seed.OfferedCars);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.Find(c => c.Make == "Audi");
      // Assert
      actual.Should().NotBeNull();
      var expected = seed.Cars.FirstOrDefault(c => c.Make == "Audi");
      expected.Should().NotBeNull();
      expected.Should().BeEquivalentTo(actual, opt => opt.IgnoringCyclicReferences());
   }
   [Fact]
   public void SelectByPredicateUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryCar.AddRange(seed.OfferedCars); 
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar
                              .Select(c => c.Make == "Volkswagen" && c.Model == "Golf");
      // Assert
      actual.Count().Should().Be(5);
      var expected = seed.Cars.Where(c => c.Make == "Volkswagen" && c.Model == "Golf")
                              .ToList();
      expected.Count().Should().Be(5);
      expected.Should().BeEquivalentTo(actual, opt => opt.IgnoringCyclicReferences());      
   }
   [Fact]
   public void SelectAllUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryCar.AddRange(seed.OfferedCars);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.SelectAll();
      // Assert
      actual.Count().Should().Be(21);
//    actual.Should().BeEquivalentTo(seed.OfferedCars, opt => opt.Excluding(c => c!.User));
      actual.Should().BeEquivalentTo(seed.OfferedCars, opt => opt.IgnoringCyclicReferences());
   }   
   [Fact]
   public void AddUt() {
      // Arrange
      var seed = new CSeed();
      _unitOfWork.RepositoryUser.Add(seed.User1);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      seed.User1.AddCar(seed.Car01);
      _unitOfWork.RepositoryCar.Add(seed.Car01);
//    _unitOfWork.RepositoryUser.Update(seed.User1);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Assert
      var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true);
      actual.Should().BeEquivalentTo(seed.User1, opt => opt.IgnoringCyclicReferences());
   }
   [Fact]
   public void CountAllCarsUt(){
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.Count();
      // Assert
      actual.Should().Be(21);
   }

   [Fact]
   public void CountCarsByMakeUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.Count("Volkswagen");
      // Assert
      actual.Should().Be(7);
   }
   [Fact]
   public void CountCarsByMakeAndModelUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.Count("Volkswagen","Golf");
      // Assert
      actual.Should().Be(5);
   }
   [Fact]
   public void SelectMakesUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.SelectMakes();
      // Assert
      var expected = new List<string> { "Audi", "BMW", "Ford", "Mercedes-Benz", "Opel", "Volkswagen"};
      actual.Should().HaveCount(6).And
                     .BeEquivalentTo(expected);
   }
   [Fact]
   public void SelectModelsUt() {
      // Arrange
      var seed = new CSeed();
      seed.InitCars();
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      var actual = _unitOfWork.RepositoryCar.SelectModels("Volkswagen");
      // Assert
      var expected = new List<string> { "Golf", "Passat", "Polo"};
      actual.Should().HaveCount(3).And.BeEquivalentTo(expected);
   }

}

//   [Fact]
//   public void User_UpdateUt() {
//      // Arrange
//      var (id, _) = RegisterUser(CreateUser());
//      // Act
//      var user = _unitOfWork.RepositoryUser.FindById(id);
//      user.Email = "m.michel@t-online.de";
//      _unitOfWork.RepositoryUser.Update(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      var actual = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.Dispose();
//      Assert.IsTrue(user.IsEqual(actual));
//   }
//   [TestMethod]
//   public void User_DeleteUt() {
//      // Arrange
//      var (id, _) = RegisterUser(CreateUser());
//      // Act
//      var user = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.RepositoryUser.Delete(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      var actual = _unitOfWork.RepositoryUser.FindById(id);
//      Assert.IsNull(actual);
//   }
//   #endregion 

//   #region One User with Address
//   [TestMethod]
//   public void UserWithAddress_FindbyIdUt() {
//      // Arrange
//      var (id, user) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var actual = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.IsTrue(user.IsEqual(actual));
//   }
//   [TestMethod]sqlloacldb









//   public void UserWithtAddress_FindUt() {
//      // Arrange
//      var (_, user) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var actual = _unitOfWork.RepositoryUser.Find(u => u.UserName =="MartinM");
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.IsTrue(user.IsEqual(actual));
//   }
//   [TestMethod]
//   public void UserWithAddress_SelectUt() {
//      // Arrange
//      var (_, user) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var actualUsers = _unitOfWork.RepositoryUser.Select(u => u.Email.Contains("gmx.de"));
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.AreEqual(1, actualUsers.Count());
//      var listOfUsers = actualUsers.ToList();
//      Assert.IsTrue(user.IsEqual(listOfUsers[0]));
//   }
//   [TestMethod]
//   public void UserWithAddress_SelectAllUt() {
//      // Arrange
//      var (_, user) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var actualUsers = _unitOfWork.RepositoryUser.SelectAll();
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.AreEqual(1, actualUsers.Count());
//      var listOfUsers = actualUsers.ToList();
//      Assert.IsTrue(user.IsEqual(listOfUsers[0]));
//   }
//   [TestMethod]
//   public void UserWithAddress_InsertUt() {
//      // Arrange
//      var user = CreateUser();
//      // Act
//      _unitOfWork.RepositoryUser.Insert(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      user.AddAddress("Herbert-Meyer-Str. 7","29556 Suderburg");
//      _unitOfWork.RepositoryUser.Update(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      var actual = _unitOfWork.RepositoryUser.FindById(user.Id);
//      _unitOfWork.Dispose();
//      Assert.IsTrue(user.IsEqual(actual));
//   }
//   [TestMethod]
//   public void UserWithAddress_UpdateUt() {
//      // Arrange
//      var (id, _) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var user = _unitOfWork.RepositoryUser.FindById(id);
//      user.Email = "m.michel@t-online.de";
//      _unitOfWork.RepositoryUser.Update(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      var actual = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.Dispose();
//      Assert.IsTrue(user.IsEqual(actual));
//   }
//   [TestMethod]
//   public void UserWithAddress_DeleteUt() {
//      // Arrange
//      var (id, _) = RegisterUserWithAddress(CreateUser());
//      // Act
//      var user = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.RepositoryUser.Delete(user);
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      var actual = _unitOfWork.RepositoryUser.FindById(id);
//      _unitOfWork.Dispose();
//      Assert.IsNull(actual);
//   }
//   #endregion

//   #region Six Users with and without Addresses
//   [TestMethod]
//   public void SixUsers_FindByIdUt() {
//      // Arrange
//      CreateUsersAndInsert();
//      // Act
//      var actual = _unitOfWork.RepositoryUser.FindById(_seed.User02.Id);
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.IsTrue(_seed.User02.IsEqual(actual));
//   }
//   [TestMethod]
//   public void SixUsers_FindUt() {
//      // Arrange
//      CreateUsersAndInsert();
//      // Act
//      var actual = _unitOfWork.RepositoryUser.Find(user => user.UserName =="BBauer");
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.IsTrue(_seed.User02.IsEqual(actual));
//   }
//   [TestMethod]
//   public void SixUsers_SelectUt() {
//      // Arrange
//      CreateUsersAndInsert();
//      // Act
//      var actualUsers = _unitOfWork.RepositoryUser.Select(u => u.Email.Contains("google.com"));
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.AreEqual(2, actualUsers.Count());
//      var user3 = actualUsers.FirstOrDefault(u => u.Id == _seed.User03.Id);
//      var user6 = actualUsers.FirstOrDefault(u => u.Id == _seed.User06.Id);
//      Assert.IsTrue(_seed.User03.IsEqual(user3));
//      Assert.IsTrue(_seed.User06.IsEqual(user6));
//   }
//   [TestMethod]
//   public void SixUsers_SelectAllUt() {
//      // Arrange
//      CreateUsersAndInsert();
//      // Act
//      var actualUsers = _unitOfWork.RepositoryUser.SelectAll();
//      _unitOfWork.SaveChanges();
//      _unitOfWork.Dispose();
//      // Assert
//      Assert.AreEqual(6, actualUsers.Count());
//   }
//   #endregion


//   static User CreateUser() {
//      var id = new Guid("11111111-2222-3333-4444-555555555555");
//      var name = "Martin Michel";
//      var email = "m.michel@gmx.de";
//      var phone = "05331 / 1234 9876";
//      var userName = "MartinM";
//      var password = "geh1m_Geh1m";
//      var salt = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
//      var hashed = AppSecurity.HashPbkdf2(password, salt);
//      var user = new User().Set(id, name, email, phone, userName, hashed, salt, Role.Customer);
//      return user;
//   }

//   (Guid, User) RegisterUser(User user) {
//      _unitOfWork.RepositoryUser.Add(user);
//      _unitOfWork.SaveAllChanges();
//      _unitOfWork.Dispose();
//      return (user.Id, user);
//   }

      
//      (int,User) RegisterUserWithAddress(User user) {
//         _unitOfWork.RepositoryUser.Insert(user);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Dispose();
//         user.AddAddress("Herbert-Meyer-Str. 7","29556 Suderburg");
//         _unitOfWork.RepositoryUser.Update(user);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Dispose();
//         return (user.Id, user);
//      }
//      void CreateUsersAndInsert() {
//         _unitOfWork.RepositoryUser.Insert(_seed.User01);
//         _unitOfWork.RepositoryUser.Insert(_seed.User02);
//         _unitOfWork.RepositoryUser.Insert(_seed.User03);
//         _unitOfWork.RepositoryUser.Insert(_seed.User04);
//         _unitOfWork.RepositoryUser.Insert(_seed.User05);
//         _unitOfWork.RepositoryUser.Insert(_seed.User06);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Dispose();

//         _unitOfWork.RepositoryUser.Attach(_seed.User02);
//         _unitOfWork.RepositoryUser.Attach(_seed.User04);
//         _unitOfWork.RepositoryUser.Attach(_seed.User06);
//         _seed.User02.AddAddress("Bahnhofstr. 1", "29525 Uelzen");
//         _seed.User04.AddAddress("Schloßplatz 23", "29227 Celle");
//         _seed.User06.AddAddress("Wallstr. 17", "21335 Lüneburg");
//         _unitOfWork.RepositoryUser.Update(_seed.User02);
//         _unitOfWork.RepositoryUser.Update(_seed.User04);
//         _unitOfWork.RepositoryUser.Update(_seed.User06);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Dispose();
//      }
//}
