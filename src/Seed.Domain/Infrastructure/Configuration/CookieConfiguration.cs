namespace TSeedDomain.Infrastructure.Configuration
{
    public class CookieConfiguration
    {
        public string Path { get; set; }
        public int ExpiryInSeconds { get; set; }
        public bool IsSecure { get; set; }
    }
}