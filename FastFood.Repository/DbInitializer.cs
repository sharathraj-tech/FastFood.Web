using FastFood.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Repository
{
    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            if (_context.Roles.Any(x => x.Name == "Admin")) return;
            _roleManager.CreateAsync(new IdentityRole("Manager"))
                .GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Admin"))
                .GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Customer"))
                .GetAwaiter().GetResult();

            var applicationUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                Name = "Admin",
                City = "XYZ",
                Address = "XYZ",
                PostalCode = "XYZ",

            };

            _userManager.CreateAsync(applicationUser, "Admin@123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(applicationUser, "Admin");
        }
    }
}
