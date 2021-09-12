using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookStore_API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            //Admin user
            //if (await userManager.FindByEmailAsync("admin@bookstore.com") == null)
            //{
                //var user = new IdentityUser
                //{
                //    UserName = "admin",
                //    Email = "admin@bookstore.com"
                //};

                //var result = await userManager.CreateAsync(user, "P@55word");

                //if(result.Succeeded)
                //{
                //    await userManager.AddToRoleAsync(user, "Administrator");
                //}
            //}

            //Sample Customer
            //if (await userManager.FindByEmailAsync("customer@gmail.com") == null)
            //{
                var user1 = new IdentityUser
                {
                    UserName = "customerOne",
                    Email = "customer@gmail.com"
                };

                var result1 = await userManager.CreateAsync(user1, "P@55word");

                if (result1.Succeeded)
                {
                    await userManager.AddToRoleAsync(user1, "Customer");
                }
            //}

            //Sample Customer
            //if (await userManager.FindByEmailAsync("customer2@gmail.com") == null)
            //{
                var user2 = new IdentityUser
                {
                    UserName = "customerTwo",
                    Email = "customer2@gmail.com"
                };

                var result2 = await userManager.CreateAsync(user2, "P@55word");

                if (result2.Succeeded)
                {
                    await userManager.AddToRoleAsync(user2, "Customer");
                }
            //}
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync("Administrator"))
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };

                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}
