using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
// using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Data
{
    public class Seed
    {

        public static async Task SeedUsers(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager){
            if(await userManager.Users.AnyAsync()) return;
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AppUser>>(userData);
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user,"Pa$$w0rd");
                await userManager.AddToRoleAsync(user,"Member");
            
            }

            var admin = new AppUser
            {
                UserName="admin"
            };

            await userManager.CreateAsync(admin , "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin , new[] {"Admin" , "Moderator"});
        }        
    }
}