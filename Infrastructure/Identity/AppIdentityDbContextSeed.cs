using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "10 The Street",
                        City = "Osgiliath",
                        State = "Gondor",
                        Zipcode = "99999"
                    }
                };
                
                // Changing password here for production publish version
                // await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.CreateAsync(user, "Nazca20!@");
            }
        }
    }
}