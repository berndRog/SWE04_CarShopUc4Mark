// Value Objects
namespace CarShop.DomainModel.ValueObjects;

public class CarTs {
   // = readonly properties
   public string Make      { get; init; } = string.Empty; 
   public string Model     { get; init; } = string.Empty;
   public double PriceFrom { get; init; } 
   public double PriceTo   { get; init; }
   public int RegFrom      { get; init; }
   public int RegTo        { get; init; }
}
