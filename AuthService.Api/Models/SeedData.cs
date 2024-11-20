using MangaScraper.Data.Data;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Api.Models
{
    public static class SeedData
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using (IServiceScope scope = app.Services.CreateScope())
            {
                // Ottengo i servizi necessari
                UserIdentityDbContext context = scope.ServiceProvider.GetService<UserIdentityDbContext>()!;
                UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>()!;
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()!;

                // Creo i ruoli
                IdentityRole roleAdmin = new IdentityRole("Admin");
                IdentityRole roleUser = new IdentityRole("User");

                // Aggiungo i ruoli
                await roleManager.CreateAsync(roleAdmin);
                await roleManager.CreateAsync(roleUser);

                // Creo gli user
                User userAdmin = new User("Admin", "admin@test.it");
                User user = new User("User", "user@test.it");

                // Aggiungo gli user con le password
                await userManager.CreateAsync(userAdmin, "33**DDss");
                await userManager.CreateAsync(user, "33**DDss");

                // Aggiungo i ruoli agli user
                await userManager.AddToRoleAsync(userAdmin, roleAdmin.Name!);
                await userManager.AddToRoleAsync(user, roleUser.Name!);
            }

            return app;
        }
    }
}
