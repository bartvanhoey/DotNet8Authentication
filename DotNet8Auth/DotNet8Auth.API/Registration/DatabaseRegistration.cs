using DotNet8Auth.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DotNet8Auth.API.Registration;

public static class DatabaseRegistration
{
    public static void RegisterDatabase(this WebApplicationBuilder webApplicationBuilder)
    {
        var connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string not found");
        webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}