namespace ShareSpace.Server.JWT
{
   public sealed class TokenSettings
   {
      public string SecretKey { get; set; } = string.Empty;
      public string Issuer { get; set; } = string.Empty;
      public string Audience { get; set; } = string.Empty;
   }
}