using MangaScraper.Data.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraper.Data.Data.ModelConfiguration
{
    public class RefreshTokenEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
        {
            //// Primary Key
            //builder.HasKey(rt => rt.JwtToken);
            //
            //// Navigation One to Many User => RefreshToken
            //builder
            //    .HasOne(rt => rt.User)
            //    .WithMany(u => u.RefreshTokens)
            //    .HasForeignKey(rt => rt.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
