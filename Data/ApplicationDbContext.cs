using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SklepAI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SklepAI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartLine> Lines { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public static async Task InitialSeed(IServiceProvider serviceProvider)
        {
            UserManager<IdentityUser> userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string username = "Administrator";
            string email = "admin@admin.com";
            string password = "admin!1";
            string adminRole = "Admin";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                if (await roleManager.FindByNameAsync(adminRole) == null)
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                if (await roleManager.FindByNameAsync("User") == null)
                    await roleManager.CreateAsync(new IdentityRole("User"));
            
                IdentityUser user = new()
                {
                    UserName = username,
                    Email = email
                };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, adminRole);
                }
            }
        }
    }
}
