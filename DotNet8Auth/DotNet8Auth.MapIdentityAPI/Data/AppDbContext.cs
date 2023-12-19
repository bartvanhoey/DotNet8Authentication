using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNet8Auth.MapIdentityAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : 
    IdentityDbContext<MyUser>(options)
{
}