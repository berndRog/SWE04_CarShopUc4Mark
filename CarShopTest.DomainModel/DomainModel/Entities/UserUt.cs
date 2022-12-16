using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using CarShop.Utilities;

namespace CarShopTest.DomainModel.Entities;

public class UserUt {

   private readonly Guid _id = new Guid("10000000-0000-0000-0000-000000000000");
   private readonly string _firstName = "Erika";
   private readonly string _lastName = "Mustermann";
   private readonly string _email = "e.mustermann@t-online.de";
   private readonly string _phone = "05826 / 1234 56";
   private readonly string _imagePath = @"C:\CarShop\images\image1.jpg";
   private readonly string _userName = "ErikaM";
   private readonly string _salt = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
   private readonly string _password;
   private readonly Role _role = Role.User;
   
   private readonly User _user;  

   public UserUt() {
      _password = AppSecurity.HashPbkdf2("geh1m_", _salt);
      _user = new User().Set(_id, _firstName, _lastName, _email, _phone, _imagePath,
         _userName, _password,_salt, Role.User);
   }

   #region ctor, getter, setter
   [Fact]
   public void CtorUt() {
      // Act
      var actual = new User();
      // Assert
      actual.Should().NotBeNull() .And. BeOfType<User>();
   }
   [Fact]
   public void CtorSetUt() {
      // Arrange
      // Act
      var actual = new User().Set(_id, _firstName, _lastName, _email, _phone, _imagePath,
         _userName, _password,_salt, Role.User);
      // Assert         
      actual.Should().NotBeNull() .And. BeOfType<User>();
      actual.Id.Should().Be(_id);
      actual.FirstName.Should().Be(_firstName);
      actual.LastName.Should().Be(_lastName);
      actual.Email.Should().Be(_email);
      actual.Phone.Should().Be(_phone);
      actual.ImagePath.Should().Be(_imagePath);
      actual.UserName.Should().Be(_userName);
      actual.Password.Should().Be(_password);
      actual.Salt.Should().Be(_salt);
      actual.Role.Should().Be(_role);
   }
   [Fact]
   public void ObjectInitializerUt() {
      // Arrange        
      // Act 
      var actual = new User {
         Id = _id,
         FirstName = _firstName,
         LastName = _lastName,
         Email = _email,
         Phone = _phone,
         ImagePath = _imagePath,
         UserName = _userName,
         Password = _password,
         Salt = _salt,
         Role = _role
      };
      // Assert
      actual.Should().NotBeNull() .And. BeOfType<User>();
      actual.Id.Should().Be(_id);
      actual.FirstName.Should().Be(_firstName);
      actual.LastName.Should().Be(_lastName);
      actual.Email.Should().Be(_email);
      actual.Phone.Should().Be(_phone);
      actual.UserName.Should().Be(_userName);
      actual.Password.Should().Be(_password);
      actual.Salt.Should().Be(_salt);
      actual.Role.Should().Be(_role);
   }
   [Fact]
   public void GetterUt() {
      // Arrange
      var actual = new User().Set(_id, _firstName, _lastName, _email, _phone, _imagePath,
         _userName, _password, _salt, Role.User);
      // Act
      var actualId = actual.Id;
      var actualFirstName = actual.FirstName;
      var actualLastName = actual.LastName;
      var actualEmail = actual.Email;
      var actualPhone = actual.Phone;
      var actualImagePath = actual.ImagePath;
      var actualUserName = actual.UserName;
      var actualPassword = actual.Password;
      var actualSalt = actual.Salt;
      var actualRole = actual.Role;
      // Assert         
      actual.Should().NotBeNull() .And .BeOfType<User>();
      actualId.Should().Be(_id);
      actualFirstName.Should().Be(_firstName);
      actualLastName.Should().Be(_lastName);
      actualEmail.Should().Be(_email);
      actualPhone.Should().Be(_phone);
      actualImagePath.Should().Be(_imagePath);
      actualUserName.Should().Be(_userName);
      actualPassword.Should().Be(_password);
      actualSalt.Should().Be(_salt);
      actualRole.Should().Be(_role);
   }
   [Fact]
   public void SetterUt() {
      //Arrange
      // Act
      var actual = new User();
      actual.Id = _id;
      actual.FirstName = _firstName;
      actual.LastName = _lastName;
      actual.Email = _email;
      actual.Phone = _phone;
      actual.ImagePath = _imagePath;
      actual.UserName = _userName;
      actual.Password = _password;
      actual.Salt = _salt;
      actual.Role = _role;
      // Assert         
      actual.Should().NotBeNull() .And .BeOfType<User>();
      actual.Id.Should().Be(_id);
      actual.FirstName.Should().Be(_firstName);
      actual.LastName.Should().Be(_lastName);
      actual.Email.Should().Be(_email);
      actual.Phone.Should().Be(_phone);
      actual.ImagePath.Should().Be(_imagePath);         
      actual.UserName.Should().Be(_userName);
      actual.Password.Should().Be(_password);
      actual.Salt.Should().Be(_salt);
      actual.Role.Should().Be(_role);
   }
   [Fact]
   public void EqualsStateEqualsUt() {
      //Arrange
      Guid id = new Guid("10000000-0000-0000-0000-000000000000");
      string  firstName = "Erika";
      string  lastName = "Mustermann";
      string  email = "e.mustermann@t-online.de";
      string? phone = "05826 / 1234 56";
      string? imagePath = @"C:\CarShop\images\image1.jpg";
      string  userName = "ErikaM";
      string  input = "Geh1m_geh1m";
      string  salt = "iOQkTANBTh+MJZUtQRdEjZkCvukcokIBoU3Q1fUEFtY=";
      string  password = AppSecurity.HashPbkdf2(input, salt);
      Role    role = Role.User;

      var user1 = new User().Set(id, firstName, lastName, email, phone, imagePath, 
         userName, password, salt, role);
      var user2 = new User().Set(id, firstName, lastName, email, phone, imagePath,
         userName, password, salt, role);
      // Act
      // are references equal ???
      var result1 = user1 == user2;
      var result2 = ReferenceEquals(user1, user2);
      //// are object (states) are equal
      var result3 = user1.Equals(user1);
      var result4 = user1.GetHashCode().Equals(user2.GetHashCode());  
      // 
   }
   #endregion

      #region Address
   [Fact]
   public void AddStreetNumberZipCityUt() {
      // Arrange
      string street = "Herbert-Meyer-String";
      string number = "7";
      string zip = "29556";
      string city = "Suderburg";
      // Act
      _user.AddOrUpdateAddress(street, number, zip, city);
      // Assert
      _user.Address?.Street.Should().Be(street);
      _user.Address?.Number.Should().Be(number);
      _user.Address?.Zip.Should().Be(zip);
      _user.Address?.City.Should().Be(city);
   }
   [Fact]
   public void AddAddressUt() {
      // Arrange
      Guid id = new Guid("a0001000-0000-0000-0000-000000000000");
      string street = "Herbert-Meyer-String";
      string number = "7";
      string zip = "29556";
      string city = "Suderburg";
      Address address = new Address().Set(id, street, number, zip, city);
      // Act
      _user.AddOrUpdateAddress(address);
      // Assert
      _user.Address.Should().BeEquivalentTo(address);
   }
   [Fact]
   public void UpdateAddressUt() {
      // Arrange
      Address address = new CSeed().Address1;
      _user.AddOrUpdateAddress(address);
      // Act
      string street = "Herbert-Meyer-String";
      string number = "7";
      string zip = "29556";
      string city = "Suderburg";      
      _user.AddOrUpdateAddress(street, number, zip, city);
      // Assert
      _user.Address?.Street.Should().Be(street);
      _user.Address?.Number.Should().Be(number);
      _user.Address?.Zip.Should().Be(zip);
      _user.Address?.City.Should().Be(city);
   }
   #endregion   


   #region Car
   [Fact]
   public void AddCarUt() {
      // Arrange
      var seed = new CSeed();
      var user = seed.User1;
      var car = seed.Car01;
      // Act
      user.AddCar(car);
      // Assert
      user.OfferedCars.Count.Should().Be(1);
      var actualCar = user.FindCarById(car.Id);
      actualCar.Should().BeEquivalentTo(car);
   }
   [Fact]
   public void RemoveCarUt() {
      // Arrange
      var seed = new CSeed();
      seed.User1.AddCar(seed.Car01);
      // Act
      seed.User1.RemoveCar(seed.Car01);
      // Assert
      var actual = seed.User1.OfferedCars as List<Car>;
      actual.Should().NotBeNull().And.HaveCount(0);
   }
   #endregion
   
}