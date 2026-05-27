// Pozovi ovu metodu iz Configuration.cs Seed metode ako koristiš EF migrations.
// Promijeni email/password po potrebi.

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HairBookPro.Models
{
    public static class SeedRolesAndAdmin
    {
        public static void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
                roleManager.Create(new IdentityRole("Admin"));

            if (!roleManager.RoleExists("User"))
                roleManager.Create(new IdentityRole("User"));

            var adminEmail = "admin@hairbookpro.com";
            var admin = userManager.FindByEmail(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                userManager.Create(admin, "Admin123!");
                userManager.AddToRole(admin.Id, "Admin");
            }
        }
    }
}
