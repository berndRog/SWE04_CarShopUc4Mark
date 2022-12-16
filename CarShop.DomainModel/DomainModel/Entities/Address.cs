namespace CarShop.DomainModel.Entities; 
public class Address: ABaseEntity {

   #region properties
   public override Guid Id { get; set; } = Guid.Empty;
   public string Street { get; set; } = "";
   public string Number { get; set; } = "";
   public string Zip { get; set; } = "";
   public string City { get; set; } = "";
   // Address --> User [1]
   public User User { get; set; } = NullUser.Instance;
   public Guid UserId { get; set; } = Guid.Empty;   // Foreign Key
   #endregion

   #region Address
   public Address() {  Id = Guid.NewGuid();}
   #endregion

   #region methods
   public Address Set(Guid id, string street, string number, string zip, string city) {
      Id = (id == Guid.Empty) ? Guid.NewGuid() : id; 
      Street = street;
      Number = number;
      Zip = zip;
      City = city;
      return this;
   }

   public string AsString() {
      return $"{Id.ToString()[0..8]} {Street} {Number}, {Zip} {City}";
   }
   #endregion
}