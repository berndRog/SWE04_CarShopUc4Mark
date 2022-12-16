using CarShop.DomainModel.Entities;
using CarShop.DomainModel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarShop.Persistence;
public class CDbContext : DbContext {
   #region fields
   private readonly ILogger<CDbContext> _logger = default!;

   public DbSet<User> Users => Set<User>();
   public DbSet<Address> Addresses => Set<Address>();
   public DbSet<Car> Cars => Set<Car>();
   public DbSet<MarkedCar> MarkedCars => Set<MarkedCar>();
   #endregion

   #region ctor
   // ctor for migration only
   public CDbContext(DbContextOptions<CDbContext> options) : base(options){ }

   public CDbContext(
      DbContextOptions<CDbContext> options,
      ILogger<CDbContext> logger
   ) : base(options){
      _logger = logger;
      _logger.LogInformation("Ctor() {HashCode}", GetHashCode());
   }
   #endregion

   #region methods
   public bool SaveAllChanges(){
      _logger.LogTrace("Before SaveChanges {HashCode}\n{Tracker}",
         GetHashCode(), ChangeTracker.DebugView.LongView);

      var records = SaveChanges();

      _logger.LogTrace("After SaveChanges {HashCode}\n{Tracker}",
         GetHashCode(), ChangeTracker.DebugView.LongView);

      return records > 0;
   }

   public void LogChangeTracker(string text){
      _logger.LogTrace("{Text}\n{Tracker}",
         text, ChangeTracker.DebugView.LongView);
   }

   //protected override void OnConfiguring(
   //   DbContextOptionsBuilder optionsBuilder
   //) {
   //   var connectionsString =  
   //      @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=CarShopUc1;Integrated Security=True;AttachDbFileName=C:\Databases\LocalDb\CarShopUc1.mdf";
   //   optionsBuilder.UseSqlServer(
   //      connectionString: connectionsString
   //   )
   //   .LogTo(message => Debug.WriteLine(message), LogLevel.Trace)
   //   .EnableSensitiveDataLogging();
   //}
   ////   //optionsBuilder
   //}

   protected override void OnModelCreating(ModelBuilder modelBuilder){
      // https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key

      // Table Users properties
      modelBuilder.Entity<User>(e => {
         e.ToTable("Users");
         e.HasKey("Id");
         e.Property<Guid>("Id")
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();
         e.Property<string>("FirstName")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<string>("LastName")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<string>("Email")
            .HasMaxLength(128)
            .HasColumnType("nvarchar(128)")
            .IsRequired();
         e.Property<string>("Phone")
            .HasMaxLength(32)
            .HasColumnType("nvarchar(32)")
            .IsRequired();
         e.Property<string>("UserName")
            .HasMaxLength(32)
            .HasColumnType("nvarchar(32)")
            .IsRequired();
         e.Property<string>("Password")
            .HasMaxLength(128)
            .HasColumnType("nvarchar(128)")
            .IsRequired();
         e.Property<string>("Salt")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<Role>("Role")
            .HasColumnType("int")
            .IsRequired();
      });

      // Table Addresses properties
      modelBuilder.Entity<Address>(e => {
         e.ToTable("Addresses");
         e.HasKey("Id");
         e.Property<Guid>("Id")
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();
         e.Property<string>("Street")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<string>("Number")
            .HasMaxLength(16)
            .HasColumnType("nvarchar(16)")
            .IsRequired();
         e.Property<string>("Zip")
            .HasMaxLength(16)
            .HasColumnType("nvarchar(16)")
            .IsRequired();
         e.Property<string>("City")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<Guid>("UserId")
            .HasColumnType("uniqueidentifier")
            .IsRequired();
         e.HasIndex("UserId");
      });

      // Table Users properties
      modelBuilder.Entity<Car>(e => {
         e.ToTable("Cars");
         e.HasKey("Id");
         e.Property<Guid>("Id")
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();
         e.Property<string>("Make")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<string>("Model")
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)")
            .IsRequired();
         e.Property<double>("Price")
            .HasColumnType("float")
            .IsRequired();
         e.Property<int>("FirstReg")
            .HasColumnType("int")
            .IsRequired();
         e.Property<string>("ImagePath")
            .HasMaxLength(256)
            .HasColumnType("nvarchar(256)");
         e.Property<Guid>("UserId")
            .HasColumnType("uniqueidentifier")
            .IsRequired();
         e.HasIndex("UserId");
      });

      // Address <-> User = One-To-ZeroOrOne Relation
      modelBuilder.Entity<Address>(e => {
         e.HasOne(a => a.User)
            .WithOne(u => u.Address)
            // Foreign Key Property UserId: Guid ( Address.Id --> User.Id [1] )
            .HasForeignKey<Address>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
         // Navigational Property user: User ( Address --> User [1] )
         e.Navigation("User");
      });

      // Car <-> User = One-To-ZeroOrMany Relation
      modelBuilder.Entity<Car>(e => {
         e.HasOne(c => c.User)
            .WithMany(u => u.OfferedCars)
            // Foreign Key Property UserId: Guid ( Car.Id --> User.Id [1] )
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
         // Navigational Properties user: User ( Car --> User [1] )
         e.Navigation(c => c.User);
      });
      // Navigational Properties offeredCars:IList<Car> ( User --> User [0..*] )
      modelBuilder.Entity<User>(e => {
         e.Navigation(u => u.OfferedCars);
      });

      // Table MarkedCars properties
      modelBuilder.Entity<MarkedCar>(e => {
         e.ToTable("MarkedCars");
         e.HasKey(m => new{ m.UserId, m.CarId });
         e.Property<Guid>("UserId")
            .HasColumnType("uniqueidentifier");
         e.Property<Guid>("CarId")
            .HasColumnType("uniqueidentifier");
         e.HasIndex(m => m.UserId);
         e.HasIndex(m => m.CarId);
      });

      // MarkedCar <-> User = One-To-ZeroOrMany Relation
      // MarkedCar <-> Car  = One-To-ZeroOrMany Relation
      modelBuilder.Entity<MarkedCar>(e => {
         e.HasOne(m => m.User)
            .WithMany(u => u.MarkedCars)
            .HasForeignKey("UserId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
         e.HasOne(m => m.Car)
            .WithMany(c => c.MarkedCars)
            .HasForeignKey("CarId")
            .OnDelete(DeleteBehavior.NoAction)
//          .OnDelete(DeleteBehavior.Cascade)
//          .OnDelete(DeleteBehavior.ClientCascade))
            .IsRequired();
         e.Navigation(m => m.User);
         e.Navigation(m => m.Car);
      });

      modelBuilder.Entity<Car>()
         .Navigation(c => c.MarkedCars);
      modelBuilder.Entity<User>()
         .Navigation(u => u.MarkedCars);

   }
   #endregion
}