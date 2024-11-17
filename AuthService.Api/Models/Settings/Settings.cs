namespace AuthService.Api.Models.Settings
{
    public class Settings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            SecretKey = configuration.GetSection("JwtSettings").GetValue<string>("SecretKey") ?? throw new Exception();
            Issuer = configuration.GetSection("JwtSettings").GetValue<string>("Issuer") ?? throw new Exception();
            Audience = configuration.GetSection("JwtSettings").GetValue<string>("Audience") ?? throw new Exception();
            ExpiryMinutes = configuration.GetSection("JwtSettings").GetValue<int>("ExpiryMinutes");
        }

    }
}
