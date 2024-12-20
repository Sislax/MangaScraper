﻿using Microsoft.Extensions.Configuration;

namespace MangaView.UI.Utiles
{
    public class Settings
    {
        public string MangaViewApiUrl { get; set; }
        public string MangaScraperApiUrl { get; set; }
        public string AuthServiceApiUrl { get; set; }
        public int NPagine {  get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }

        public Settings(IConfigurationRoot clientConfiguration)
        {
            MangaViewApiUrl = clientConfiguration.GetSection("ApiSettings").GetValue<string>("MangaViewApiUrl") ?? throw new Exception("MangaViewApiUrl non trovato in configuration.");
            MangaScraperApiUrl = clientConfiguration.GetSection("ApiSettings").GetValue<string>("MangaScraperApiUrl") ?? throw new Exception("MangaScraperApiUrl non trovato in configuration");
            AuthServiceApiUrl = clientConfiguration.GetSection("ApiSettings").GetValue<string>("AuthServiceApiUrl") ?? throw new Exception("AuthServiceApiUrl non trovato in configuration.");
            NPagine = clientConfiguration.GetSection("ScrapingSettings").GetValue<int>("NPagine");
            SecretKey = clientConfiguration.GetSection("JwtSettings").GetValue<string>("SecretKey") ?? throw new Exception();
            Issuer = clientConfiguration.GetSection("JwtSettings").GetValue<string>("Issuer") ?? throw new Exception();
            Audience = clientConfiguration.GetSection("JwtSettings").GetValue<string>("Audience") ?? throw new Exception();
            ExpiryMinutes = clientConfiguration.GetSection("JwtSettings").GetValue<int>("ExpiryMinutes");
        }
    }
}
