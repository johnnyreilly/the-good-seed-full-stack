namespace TSeedDomain.Infrastructure.Configuration
{
    public class SecurityConfiguration
    {
        public CookieConfiguration Cookie { get; set; }
        public string GcnHeaderKey { get; set; }
    }
}