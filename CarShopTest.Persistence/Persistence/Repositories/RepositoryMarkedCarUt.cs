using CarShop;
using CarShop.DomainModel.Entities;
using CarShop.Persistence;
using CarShop.Utilities;
using FluentAssertions;
using FluentAssertions.Equivalency;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CarShopTest.Persistence.Repositories;

[Collection(nameof(SystemTestCollectionDefinition))]
public class RepositoryMarkedCarUt {

   private readonly IUnitOfWork _unitOfWork;

   public RepositoryMarkedCarUt() {
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
   public void AddUt() {
      // Arrange
      var seed = new CSeed();
      TwoUserWithTwoCars(seed);
      // Act
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      _unitOfWork.RepositoryUser.Attach(seed.User6);
      MarkedCar markedCar = seed.User1.AddMarkedCar(seed.Car21);
      _unitOfWork.RepositoryMarkedCar.Add(markedCar);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Assert
      var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      actual.Should().NotBeNull();
      actual!.MarkedCars.Should().NotBeNull().And.HaveCount(1);
      actual!.MarkedCars.Should().BeEquivalentTo(seed.User1.MarkedCars, ExcludePropMarkedCar);
   }

   private EquivalencyAssertionOptions<MarkedCar> ExcludePropMarkedCar(
      EquivalencyAssertionOptions<MarkedCar> opt
   ) {
      opt.Excluding(m => m.Car.User);
      opt.Excluding(m => m.User.OfferedCars);
      opt.Excluding(m => m.User.MarkedCars);
      opt.IgnoringCyclicReferences();
      return opt;
   }

   [Fact]
   public void RemoveUt() {
      // Arrange
      var seed = new CSeed();
      TwoUserWithTwoCars(seed);

      _unitOfWork.RepositoryUser.Attach(seed.User1);
      _unitOfWork.RepositoryUser.Attach(seed.User6);
      MarkedCar markedCar = seed.User1.AddMarkedCar(seed.Car21);
      _unitOfWork.RepositoryMarkedCar.Add(markedCar);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Act
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      seed.User1.RemoveMarkedCar(seed.Car21);
      _unitOfWork.RepositoryMarkedCar.Remove(markedCar);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Assert
      var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      actual.Should().NotBeNull();
      var markedCars = actual!.MarkedCars;
      markedCars.Should().NotBeNull().And.HaveCount(0);
      actual!.MarkedCars.Should().BeEquivalentTo(seed.User1.MarkedCars, ExcludePropMarkedCar);
   }

   [Fact]
   public void AddRangeUt() {
      // Arrange
      var seed = new CSeed();
      SixUsersWith21Cars(seed);
      // Act
      MarkSixCarsForUser1(seed);
      // Assert
      var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      actual.Should().NotBeNull();
      var markedCars = actual!.MarkedCars;
      markedCars.Should().NotBeNull().And.HaveCount(6);
      actual!.MarkedCars.Should().BeEquivalentTo(seed.User1.MarkedCars, ExcludePropMarkedCar);
   }

   [Fact]
   public void SelectMarkedCarsByUserIdUt() {
      // Arrange
      var seed = new CSeed();
      SixUsersWith21Cars(seed);
      MarkSixCarsForUser1(seed);
      // Act
      var actual = _unitOfWork.RepositoryMarkedCar.SelectByUserId(seed.User1.Id);
      // Assert
      //var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      //actual.Should().NotBeNull();
      //var markedCars = actual!.MarkedCars;
      //markedCars.Should().NotBeNull().And.HaveCount(6);
      //  actual.Should().BeEquivalentTo(seed.User1, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void SelectMarkedCarsByCarIdUt() {
      // Arrange
      var seed = new CSeed();
      SixUsersWith21Cars(seed);
      MarkSixCarsForUser1(seed);
      // Act
      var actual = _unitOfWork.RepositoryMarkedCar.SelectByCarId(seed.Car21.Id);
      // Assert
      actual.Should().BeEquivalentTo(seed.User1.MarkedCars, opt => opt.IgnoringCyclicReferences());
      //var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      //actual.Should().NotBeNull();
      //var markedCars = actual!.MarkedCars;
      //markedCars.Should().NotBeNull().And.HaveCount(6);
      //  actual.Should().BeEquivalentTo(seed.User1, opt => opt.IgnoringCyclicReferences());
   }

   [Fact]
   public void RemoveCarUt() {
      // Arrange
      var seed = new CSeed();
      SixUsersWith21Cars(seed);
      MarkSixCarsForUser1(seed);
      // Act
      IEnumerable<MarkedCar> markedCars = _unitOfWork.RepositoryMarkedCar.SelectByCarId(seed.Car21.Id);
      foreach(var m in markedCars) {
         User user = m.User;
         Car car = m.Car;
         user.RemoveMarkedCar(car);
      }
        
      seed.User6.RemoveCar(seed.Car21);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();


      // Assert
      var actual = _unitOfWork.RepositoryUser.FindById(seed.User1.Id, true, true, true);
      actual.Should().NotBeNull();
      //var markedCars = actual!.MarkedCars;
      //markedCars.Should().NotBeNull().And.HaveCount(6);


      //  actual.Should().BeEquivalentTo(seed.User1, opt => opt.IgnoringCyclicReferences());

   }

   private void TwoUserWithTwoCars(CSeed seed) {
      // Add User1 and User6
      _unitOfWork.RepositoryUser.Add(seed.User1);
      _unitOfWork.RepositoryUser.Add(seed.User6);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Add Car01 to User1
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      seed.User1.AddCar(seed.Car01);
      _unitOfWork.RepositoryUser.Update(seed.User1);

      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Add Car21 to User6
      _unitOfWork.RepositoryUser.Attach(seed.User6);
      seed.User6.AddCar(seed.Car21);
      _unitOfWork.RepositoryCar.Add(seed.Car21);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
   }

   private void SixUsersWith21Cars(CSeed seed) {
      // Add 6 users
      _unitOfWork.RepositoryUser.AddRange(seed.Users);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
      // Add Car01 to User1
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      seed.User1.AddCar(seed.Car01);
      _unitOfWork.RepositoryCar.Add(seed.Car01);
//    _unitOfWork.RepositoryUser.Update(seed.User1);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

      // Add Car02&Car03 to User2 ( with 
      _unitOfWork.RepositoryUser.Attach(seed.User2);
      seed.User2.AddCar(seed.Car02);
      seed.User2.AddCar(seed.Car03);
      _unitOfWork.RepositoryCar.Add(seed.Car02);     // Add cars via add car
      _unitOfWork.RepositoryCar.Add(seed.Car03);     // Add cars via add car
//    _unitOfWork.RepositoryUser.Update(seed.User2);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

      // Add Car04&Car05&Car06 to User3
      _unitOfWork.RepositoryUser.Attach(seed.User3);
      seed.User3.AddCar(seed.Car04);
      seed.User3.AddCar(seed.Car05);
      seed.User3.AddCar(seed.Car06);
//    _unitOfWork.RepositoryCar.Add(seed.Car04);
//    _unitOfWork.RepositoryCar.Add(seed.Car05);
//    _unitOfWork.RepositoryCar.Add(seed.Car06);
      _unitOfWork.RepositoryUser.Update(seed.User3);  // Add cars via update user
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

      // Add Car07&Car08&Car09&Car10 to User4
      _unitOfWork.RepositoryUser.Attach(seed.User4);
      seed.User4.AddCar(seed.Car07);
      seed.User4.AddCar(seed.Car08);
      seed.User4.AddCar(seed.Car09);
      seed.User4.AddCar(seed.Car10);
      _unitOfWork.RepositoryUser.Update(seed.User4);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

      // Add Car11&Car12&Car13&Car14&Car15 to User5
      _unitOfWork.RepositoryUser.Attach(seed.User5);
      seed.User5.AddCar(seed.Car11);
      seed.User5.AddCar(seed.Car12);
      seed.User5.AddCar(seed.Car13);
      seed.User5.AddCar(seed.Car14);
      seed.User5.AddCar(seed.Car15);
      _unitOfWork.RepositoryUser.Update(seed.User5);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

      // Add Car16&Car17&Car18&Car19&Car20&Car21 to User6
      _unitOfWork.RepositoryUser.Attach(seed.User6);
      seed.User6.AddCar(seed.Car16);
      seed.User6.AddCar(seed.Car17);
      seed.User6.AddCar(seed.Car18);
      seed.User6.AddCar(seed.Car19);
      seed.User6.AddCar(seed.Car20);
      seed.User6.AddCar(seed.Car21);
      _unitOfWork.RepositoryUser.Update(seed.User6);
      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();
   }

   private void MarkSixCarsForUser1(CSeed seed) {
      _unitOfWork.RepositoryUser.Attach(seed.User1);
      _unitOfWork.RepositoryUser.Attach(seed.User6);

      MarkedCar mc1 = seed.User1.AddMarkedCar(seed.Car21);
      MarkedCar mc2 = seed.User1.AddMarkedCar(seed.Car20);
      MarkedCar mc3 = seed.User1.AddMarkedCar(seed.Car19);
      MarkedCar mc4 = seed.User1.AddMarkedCar(seed.Car18);
      MarkedCar mc5 = seed.User1.AddMarkedCar(seed.Car17);
      MarkedCar mc6 = seed.User1.AddMarkedCar(seed.Car16);

      _unitOfWork.RepositoryMarkedCar.Add(mc1);
      _unitOfWork.RepositoryMarkedCar.Add(mc2);
      _unitOfWork.RepositoryMarkedCar.Add(mc3);
      _unitOfWork.RepositoryMarkedCar.Add(mc4);
      _unitOfWork.RepositoryMarkedCar.Add(mc5);
      _unitOfWork.RepositoryMarkedCar.Add(mc6);

      _unitOfWork.SaveAllChanges();
      _unitOfWork.Dispose();

   }
}