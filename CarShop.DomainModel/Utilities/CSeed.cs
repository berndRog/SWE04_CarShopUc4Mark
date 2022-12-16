using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;

namespace CarShop.Utilities; 

public class CSeed {

   public User DefaultUser { get; }
   public User User1 { get; }
   public User User2 { get; }
   public User User3 { get; }
   public User User4 { get; }
   public User User5 { get; }
   public User User6 { get; }
   public List<User> Users = new();

   public Address DefaultAddress { get; }
   public Address Address1 { get; }
   public Address Address2 { get; }
   public Address Address3 { get; }

   public Car DefaultCar { get; }
   public Car Car01 { get; }
   public Car Car02 { get; }
   public Car Car03 { get; }
   public Car Car04 { get; }
   public Car Car05 { get; }
   public Car Car06 { get; }
   public Car Car07 { get; }
   public Car Car08 { get; }
   public Car Car09 { get; }
   public Car Car10 { get; }
   public Car Car11 { get; }
   public Car Car12 { get; }
   public Car Car13 { get; }
   public Car Car14 { get; }
   public Car Car15 { get; }
   public Car Car16 { get; }
   public Car Car17 { get; }
   public Car Car18 { get; }
   public Car Car19 { get; }
   public Car Car20 { get; }
   public Car Car21 { get; }
   public Car Car22 { get; }
   public Car Car23 { get; }
   public Car Car24 { get; }
   public Car Car25 { get; }  
   public Car Car26 { get; }
   public Car Car27 { get; }
   public Car Car28 { get; }
   public Car Car29 { get; }
   public Car Car30 { get; }
   public List<Car> Cars = new();
   public List<Car> OfferedCars = new();


   public CSeed() {
      var password = "geh1m_";
      var salt = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
      var hashed = AppSecurity.HashPbkdf2(password, salt);

      DefaultUser = new User().Set(Utils.CrGuid("defa"), "Erika", "Mustermann", "e.mustermann@t-online.de", "0987 654321",
      null, "ErikaM", hashed, salt, Role.User);
      User1 = new User().Set(Utils.CrGuid("0001"), "Achim", "Arndt", "a.arndt@t-online.de", "0123 456 7890",
         @"C:\CarShop\images\image1.jpg", "AchimA", hashed, salt, Role.User);
      User2 = new User().Set(Utils.CrGuid("0002"), "Berhard", "Bauer", "b.bauer@outlook.com", "0234 567 8902",
         @"C:\CarShop\images\image2.jpg", "BBauer", hashed, salt, Role.User);
      User3 = new User().Set(Utils.CrGuid("0003"), "Christine", "Conrad", "c.conrad@google.com", "0345 678 9012",
         @"C:\CarShop\images\image3.jpg", "ChriCon", hashed, salt, Role.User);
      User4 = new User().Set(Utils.CrGuid("0004"), "Dagmar", "Deppe", "d.deppe@freenet.de", "0456 789 0123",
         null, "DagDep", hashed, salt, Role.User);
      User5 = new User().Set(Utils.CrGuid("0005"), "Emil", "Erdmann", "e.erdmann@icloud.com", "0567 890 1234",
         null, "EmilErd", hashed, salt, Role.User);
      User6 = new User().Set(Utils.CrGuid("0006"), "Fritz", "Fischer", "f.fischer@google.com", "0678 901 2345",
         null, "FFischer", hashed, salt, Role.User);
      Users.AddRange(new List<User> { User1, User2, User3, User4, User5, User6 });

      DefaultAddress = new Address().Set(Utils.CrGuid("a000"), "Herbert-Meyer-Str.", "7", "29556", "Suderburg");
      Address1 = new Address().Set(Utils.CrGuid("a001"), "Bahnhofstr.", "1", "29525", "Uelzen");
      Address2 = new Address().Set(Utils.CrGuid("a002"), "Schloßplatz", "23", "29227", "Celle");
      Address3 = new Address().Set(Utils.CrGuid("a003"), "Wallstr.", "17", "21335", "Lüneburg");

      DefaultCar = new Car().Set(Utils.CrGuid("c000"), "Ford", "Focus", 9999, 2015, null, NullUser.Instance);
      Car01 = new Car().Set(Utils.CrGuid("c001"), "Volkswagen", "Passat", 19500, 2017, null, NullUser.Instance);
      Car02 = new Car().Set(Utils.CrGuid("c002"), "Opel", "Astra", 21000, 2018, null, NullUser.Instance);
      Car03 = new Car().Set(Utils.CrGuid("c003"), "Audi", "A4", 26800, 2019, null, NullUser.Instance);
      Car04 = new Car().Set(Utils.CrGuid("c004"), "Volkswagen", "Golf", 10000, 2012, null, NullUser.Instance);
      Car05 = new Car().Set(Utils.CrGuid("c005"), "Mercedes-Benz", "B200", 18000, 2015, null, NullUser.Instance);
      Car06 = new Car().Set(Utils.CrGuid("c006"), "Volkswagen", "Golf", 12500, 2020, null, NullUser.Instance);
      Car07 = new Car().Set(Utils.CrGuid("c007"), "Volkswagen", "Polo", 9130, 2013, null, NullUser.Instance);
      Car08 = new Car().Set(Utils.CrGuid("c008"), "Volkswagen", "Golf", 7000, 2010, null, NullUser.Instance);
      Car09 = new Car().Set(Utils.CrGuid("c009"), "BMW", "330", 28900, 2015, null, NullUser.Instance);
      Car10 = new Car().Set(Utils.CrGuid("c010"), "Volkswagen", "Golf", 8900, 2014, null, NullUser.Instance);
      Car11 = new Car().Set(Utils.CrGuid("c011"), "Volkswagen", "Golf", 11000, 2015, null, NullUser.Instance);
      Car12 = new Car().Set(Utils.CrGuid("c012"), "Audi", "A4", 36900, 2021, null, NullUser.Instance);
      Car13 = new Car().Set(Utils.CrGuid("c013"), "Audi", "A6", 108904, 2022, null, NullUser.Instance);
      Car14 = new Car().Set(Utils.CrGuid("c014"), "Audi", "S8", 166330, 2022, null, NullUser.Instance);
      Car15 = new Car().Set(Utils.CrGuid("c015"), "Audi", "Q5", 58760, 2020, null, NullUser.Instance);
      Car16 = new Car().Set(Utils.CrGuid("c016"), "BMW", "X3", 52700, 2018, null, NullUser.Instance);
      Car17 = new Car().Set(Utils.CrGuid("c017"), "BMW", "X5", 74900, 2018, null, NullUser.Instance);
      Car18 = new Car().Set(Utils.CrGuid("c018"), "BMW", "320", 36450, 2020, null, NullUser.Instance);
      Car19 = new Car().Set(Utils.CrGuid("c019"), "Ford", "Focus", 23500, 2020, null, NullUser.Instance);
      Car20 = new Car().Set(Utils.CrGuid("c020"), "Ford", "Kuga", 26990, 2020, null, NullUser.Instance);
      Car21 = new Car().Set(Utils.CrGuid("c021"), "Opel", "Astra", 21290, 2021, null, NullUser.Instance);
      Car22 = new Car().Set(Utils.CrGuid("c022"), "Opel", "Insignia", 34900, 2020, null, NullUser.Instance);
      Car23 = new Car().Set(Utils.CrGuid("c023"), "Opel", "Mokka", 25990, 2021, null, NullUser.Instance);
      Car24 = new Car().Set(Utils.CrGuid("c024"), "Opel", "Grandland X", 21880, 2019, null, NullUser.Instance);
      Car25 = new Car().Set(Utils.CrGuid("c025"), "Mercedes-Benz", "C 220", 64500, 2021, null, NullUser.Instance);
      Car26 = new Car().Set(Utils.CrGuid("c026"), "Mercedes-Benz", "C 180", 30880, 2017, null, NullUser.Instance);
      Car27 = new Car().Set(Utils.CrGuid("c027"), "Mercedes-Benz", "E 220", 43950, 2018, null, NullUser.Instance);
      Car28 = new Car().Set(Utils.CrGuid("c028"), "Porsche", "911", 67980, 2007,  null, NullUser.Instance);
      Car29 = new Car().Set(Utils.CrGuid("c029"), "Porsche", "Cayenne", 117400, 2020, null, NullUser.Instance);
      Car30 = new Car().Set(Utils.CrGuid("c030"), "Volkswagen", "T-Roc", 29900, 2021, null, NullUser.Instance);
      Cars.AddRange(
         new List<Car> { 
            Car01, Car02, Car03, Car04, Car05, Car06, Car07, Car08, Car09, Car10, 
            Car11, Car12, Car13, Car14, Car15, Car16, Car17, Car18, Car19, Car20,
            Car21, Car22, Car23, Car24, Car25, Car26, Car27, Car28, Car29, Car30
         });
      OfferedCars.AddRange(
         new List<Car> {
            Car01, Car02, Car03, Car04, Car05, Car06, Car07, Car08, Car09, Car10,
            Car11, Car12, Car13, Car14, Car15, Car16, Car17, Car18, Car19, Car20,
            Car21
         });
   }


   public CSeed InitAddresses() {
      User1.AddOrUpdateAddress(Address1);
      User2.AddOrUpdateAddress(Address2);
      User3.AddOrUpdateAddress(Address3);
      return this;
   }

   public CSeed InitCars() {
      User1.AddCar(Car01);

      User2.AddCar(Car02);
      User2.AddCar(Car03);

      User3.AddCar(Car04);
      User3.AddCar(Car05);
      User3.AddCar(Car06);

      User4.AddCar(Car07);
      User4.AddCar(Car08); 
      User4.AddCar(Car09); 
      User4.AddCar(Car10);

      User5.AddCar(Car11);
      User5.AddCar(Car12);
      User5.AddCar(Car13);
      User5.AddCar(Car14);
      User5.AddCar(Car15);

      User6.AddCar(Car16);
      User6.AddCar(Car17);
      User6.AddCar(Car18);
      User6.AddCar(Car19);
      User6.AddCar(Car20);
      User6.AddCar(Car21);
      return this;
   }


   public CSeed InitMarkedCars() {
      
      User1.AddMarkedCar(Car21);
      User1.AddMarkedCar(Car20);
      User1.AddMarkedCar(Car19);
      User1.AddMarkedCar(Car18);
      User1.AddMarkedCar(Car17);
      User1.AddMarkedCar(Car16);

      User2.AddMarkedCar(Car18);
      User2.AddMarkedCar(Car17);
      User2.AddMarkedCar(Car16);
      User2.AddMarkedCar(Car15);
      User2.AddMarkedCar(Car14);

      User3.AddMarkedCar(Car16);
      User3.AddMarkedCar(Car15);
      User3.AddMarkedCar(Car14);
      User3.AddMarkedCar(Car13);

      return this;
   }
} 