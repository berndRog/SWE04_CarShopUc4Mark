using CarShop.Utilities;
namespace CarShop.Test.Utilities; 

public class AppSecurityUt {


   [Fact]
   public void GenereateSaltUt() {
      // Arrange
      // Act
      var salt = AppSecurity.GenerateSalt(24);
      // Assert
      var byteArray = Convert.FromBase64String(salt);
      byteArray.Length.Should().Be(24);
   }
   [Fact]
   public void HashPbkdf2Ut() {
      // Arrange
      var salt = "1yVma82d4d8DBpuiXQ7+WCs6A/1lqv7h";
      var password = "Alle meine Entchen schwimmen auf dem See";
      // Act
      var hashed = AppSecurity.HashPbkdf2(password, salt);
      // Assert
      var expected = @"LlYaT19NiKyKKx7Q007fhjQ60a3/AGw03cvRcV5wJIN1pNA/suFZm356EFalgdCOR/ZDsFBzjzHxoJhM4HUHNw==";
      expected.Should().BeEquivalentTo(hashed);
   }
   [Fact]
   public void HashPasswordAndCompareUt() {
      // Arrange
      var password = "Alle meine Entchen schwimmen auf dem See";
      // Act
      var (salt, hashed) = AppSecurity.HashPassword(password);
      // Assert
      var result = AppSecurity.Compare(password, hashed, salt);
      result.Should().BeTrue();
   }
}