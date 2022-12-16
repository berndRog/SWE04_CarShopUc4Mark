using CarShop.DomainModel.Entities;

namespace CarShopTest.DomainModel.Entities;
public class CarUt {

   Guid _id = new Guid("c0000001-0000-0000-0000-000000000000");
   const string _make = "Ford";
   const string _model = "Fiesta";
   const double _price = 1234.0;
   const int _firstReg = 2011;
   const string? _imagePath = null;

   public CarUt() { }

   [Fact]
   public void CtorUt() {
      // Act
      var actual = new Car();
      // Assert
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Car>();
   }
   [Fact]
   public void CtorSetUt() {
      // Arrange
      // Act  ctor + set
      var actual = new Car()
         .Set(_id, _make, _model, _price, _firstReg, _imagePath, NullUser.Instance);
      // Act getter
      var actualId = actual.Id;
      var actualMake = actual.Make;
      var actualModel = actual.Model;
      var actualPrice = actual.Price;
      var actualFirstReg = actual.FirstReg;
      var actualImagePath = actual.ImagePath;
      var actualUser = actual.User;
      var actualUserId = actual.UserId;
      // Assert         
      actual.Should().NotBeNull();
      actual.Should().BeOfType<Car>();
      actualId.Should().Be(_id);
      actualMake.Should().Be(_make);
      actualModel.Should().Be(_model);
      actualPrice.Should().Be(_price);
      actualFirstReg.Should().Be(_firstReg);
      actualImagePath.Should().BeEquivalentTo(_imagePath);
      actualUser.Should().BeEquivalentTo(NullUser.Instance);
      actualUserId.Should().Be(NullUser.Instance.Id);
   }
   [Fact]
   public void CtorObjectInitializerUt() {
      // Arrange
      // Act 
      var actual = new Car {
         Id = _id,
         Make = _make,
         Model = _model,
         Price = _price,
         FirstReg = _firstReg,
         ImagePath = _imagePath,
         User = NullUser.Instance,
         UserId = NullUser.Instance.Id
      };
      // Assert         
      actual.Should().NotBeNull().And.BeOfType<Car>();
      actual.Id.Should().Be(_id);
      actual.Make.Should().Be(_make);
      actual.Model.Should().Be(_model);
      actual.Price.Should().Be(_price);
      actual.FirstReg.Should().Be(_firstReg);
      actual.ImagePath.Should().BeEquivalentTo(_imagePath);
      actual.User.Should().BeEquivalentTo(NullUser.Instance);
      actual.UserId.Should().Be(NullUser.Instance.Id);
   }
}