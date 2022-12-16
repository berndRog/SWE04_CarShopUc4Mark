
using CarShop.DomainModel.ValueObjects;
namespace CarShopTest.DomainModel.ValueObjects {

   public class CarTsUt {
      
      private readonly string _make = "Volkswagen";
      private readonly string _model = "Golf";
      private readonly double _priceFrom = 8000.0;
      private readonly double _priceTo = 10_000.0;
      private readonly int    _regFrom = 2012;
      private readonly int    _regTo   = 2015;
       
      [Fact]
      public void CtorObjectInitializerUt() {
         // Arrange
         // Act
         var actual = new CarTs {
            Make = _make,  
            Model = _model,
            PriceFrom = _priceFrom,
            PriceTo = _priceTo,
            RegFrom = _regFrom,
            RegTo = _regTo
         };
         // Assert
         actual.Should().NotBeNull() .And. BeOfType<CarTs>();
         actual.Make.Should().Be(_make);
         actual.Model.Should().Be(_model);
         actual.PriceFrom.Should().Be(_priceFrom);
         actual.PriceTo.Should().Be(_priceTo);
         actual.RegFrom.Should().Be(_regFrom);
         actual.RegTo.Should().Be(_regTo);
      }

      [Fact]
      public void GetterUt() {
         // Arrange
         var actual = new CarTs {
            Make = _make,  
            Model = _model,
            PriceFrom = _priceFrom,
            PriceTo = _priceTo,
            RegFrom = _regFrom,
            RegTo = _regTo
         }; 
         // Act
         var actualMake = actual.Make;
         var actualModel = actual.Model;
         var actualPriceFrom = actual.PriceFrom;
         var actualPriceTo = actual.PriceTo;
         var actualRegFrom = actual.RegFrom;
         var actualRegTo = actual.RegTo;
         // Assert
         actualMake.Should().Be(_make);
         actualModel.Should().Be(_model);
         actualPriceFrom.Should().Be(_priceFrom);
         actualPriceTo.Should().Be(_priceTo);
         actualRegFrom.Should().Be(_regFrom);
         actualRegTo.Should().Be(_regTo);
      }

   }
}