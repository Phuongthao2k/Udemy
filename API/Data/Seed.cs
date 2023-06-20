using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Data;
using API.Entities;

namespace API;

public class Seed
{
    public static async Task SeedUsers(DataContext dataContext) 
    {
        if(dataContext.Users.Any()) return;

        var data = await File.ReadAllTextAsync("Data/UserSeedData.json");

        var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<AppUser>>(data,options);

        foreach (var user in  users)
        {
            using var hmac = new HMACSHA512();
            user.UserName = user.UserName.ToLower();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Passw0rd"));
            user.PasswordSalt = hmac.Key;

            dataContext.Users.Add(user);


        }

        await dataContext.SaveChangesAsync();
    }
}
