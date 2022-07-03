using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RSWEBProject.Data;
using RSWEBProject.Areas.Identity.Data;
namespace RSWEBProject.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<RSWEBProjectUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            RSWEBProjectUser user = await UserManager.FindByEmailAsync("admin@project.com");
            if (user == null)
            {
                var User = new RSWEBProjectUser();
                User.Email = "admin@project.com";
                User.UserName = "admin@project.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            //Add Delivery Man Role
            roleCheck = await RoleManager.RoleExistsAsync("Delivery Man");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Delivery Man")); }
            user = await UserManager.FindByEmailAsync("deliveryman@project.com");
            if (user == null)
            {
                var User = new RSWEBProjectUser();
                User.Email = "deliveryman@project.com";
                User.UserName = "deliveryman@project.com";
                string userPWD = "Deliveryman123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Delivery Man
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Delivery Man"); }
            }
            //Add Client Role
            roleCheck = await RoleManager.RoleExistsAsync("Client");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Client")); }
            user = await UserManager.FindByEmailAsync("client@project.com");
            if (user == null)
            {
                var User = new RSWEBProjectUser();
                User.Email = "client@project.com";
                User.UserName = "client@project.com";
                string userPWD = "Client123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Client
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Client"); }
            }
        }


        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RSWEBProjectContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RSWEBProjectContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any Restaurants.
                if (context.Restaurant.Any() || context.DeliveryMan.Any() || context.Client.Any())
                {
                    return;   // DB has been seeded
                }

                context.DeliveryMan.AddRange(
                    new DeliveryMan { /*Id = 1, */FirstName = "Rob", LastName = "Reiner", RSWEBProjectUserId = context.Users.Single(x => x.Email == "deliveryman@project.com").Id },
                    new DeliveryMan { /*Id = 2, */FirstName = "Ivan", LastName = "Reitman" },
                    new DeliveryMan { /*Id = 3, */FirstName = "Howard", LastName = "Hawks" }
                );
                context.SaveChanges();

                context.Client.AddRange(
                    new Client { /*Id = 1, */FirstName = "Billy", LastName = "Crystal", RSWEBProjectUserId = context.Users.Single(x => x.Email == "client@project.com").Id },
                    new Client { /*Id = 2, */FirstName = "Meg", LastName = "Ryan" },
                    new Client { /*Id = 3, */FirstName = "Carrie", LastName = "Fisher" },
                    new Client { /*Id = 4, */FirstName = "Bill", LastName = "Murray" },
                    new Client { /*Id = 5, */FirstName = "Dan", LastName = "Aykroyd"},
                    new Client { /*Id = 6, */FirstName = "Sigourney", LastName = "Weaver" },
                    new Client { /*Id = 7, */FirstName = "John", LastName = "Wayne" },
                    new Client { /*Id = 8, */FirstName = "Dean", LastName = "Martin" }
                );
                context.SaveChanges();

                context.Restaurant.AddRange(
                    new Restaurant
                    {
                        //Id = 1,
                        Name="McDonalds",
                        DeliveryManId = context.DeliveryMan.Single(d => d.FirstName == "Rob" && d.LastName == "Reiner").Id
                    },
                    new Restaurant
                    {
                        //Id = 2,
                        Name="Staro Bure",
                        DeliveryManId = context.DeliveryMan.Single(d => d.FirstName == "Ivan" && d.LastName == "Reitman").Id
                    },
                    new Restaurant
                    {
                        //Id = 3,
                        Name="Kentucky Fried Chicken",
                        DeliveryManId = context.DeliveryMan.Single(d => d.FirstName == "Ivan" && d.LastName == "Reitman").Id
                    },
                    new Restaurant
                    {
                        //Id = 4,
                        Name="Ana Kristi",
                        DeliveryManId = context.DeliveryMan.Single(d => d.FirstName == "Howard" && d.LastName == "Hawks").Id
                    }
                );
                context.SaveChanges();

                context.Order.AddRange(
                    new Order { ClientId = 1, RestaurantId = 1, SerialNumber = 1 },
                    new Order { ClientId = 2, RestaurantId = 1, SerialNumber = 2 },
                    new Order { ClientId = 3, RestaurantId = 1, SerialNumber = 3 },
                    new Order { ClientId = 4, RestaurantId = 2, SerialNumber = 4 },
                    new Order { ClientId = 5, RestaurantId = 2, SerialNumber = 5 },
                    new Order { ClientId = 6, RestaurantId = 2, SerialNumber = 6 },
                    new Order { ClientId = 4, RestaurantId = 3, SerialNumber = 7 },
                    new Order { ClientId = 5, RestaurantId = 3, SerialNumber = 8 },
                    new Order { ClientId = 6, RestaurantId = 3, SerialNumber = 9 },
                    new Order { ClientId = 7, RestaurantId = 4, SerialNumber = 10 },
                    new Order { ClientId = 8, RestaurantId = 4, SerialNumber = 11 }
                );
                context.SaveChanges();
            }
        }
    }
}