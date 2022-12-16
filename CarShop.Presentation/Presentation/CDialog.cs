using CarShop.DomainModel.ValueObjects;

namespace CarShop.Presentation;

internal class CDialog: IDialog {

   private readonly IAppCore _appCore;
   private readonly IAppUser _appUser;

               // Dependency Injection via Ctor
   public CDialog(
      IAppCore appCore,
      IAppUser appUser
   ) {
      _appCore = appCore;
      _appUser = appUser;
   }
   
   public void ShowDialog() {
      //var count = _application.CountCars();
      //Console.WriteLine($"\nTreffer{count,3}");
   
      //var makes = _application.ReadMakes();
      //Console.WriteLine($"\nMarkenliste");
      //foreach(var make in makes) Console.WriteLine($"{make}");
  
      //string selectedMake = "Volkswagen";
      //var models = _application.ReadModels(selectedMake);
      //Console.WriteLine($"\nModelliste");
      //foreach(var model in models) Console.WriteLine($"{model}");
   
      //var carTs = new CarTs {
      //   Make = "Volkswagen",
      //   Model = "Golf",
      //   PriceFrom = 8_000.0,
      //   PriceTo = 10_000.0,
      //   RegFrom = 2010,
      //   RegTo = 2022
      //};
      //var cars      = _application.ReadCars(carTs);
      //Console.WriteLine($"\nAusgewählt: {carTs.Make} {carTs.Model}");
      //foreach(var car in cars) Console.WriteLine($"{car.AsString()}");
   }
}