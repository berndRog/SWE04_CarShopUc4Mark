namespace CarShop.Utilities; 

public static class Utils {

   public static Guid CrGuid(string f4)
      => new Guid($"{f4}0000-0000-0000-0000-000000000000");

   public static string Guid8(Guid guid) => guid.ToString()[..8];

   public static string UtcTimeNowString =>
      DateTime.Now.ToUniversalTime()
              .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

   public static string ToUtcTimeString(DateTime dateTime) {
      return dateTime.ToUniversalTime()
                     .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
   }
}
