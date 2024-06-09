namespace DotNet8Auth.API.Registration;

public static class CorsPolicyRegistration
{
    
    public static void AddCorsPolicy(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            var origins = builder.Configuration.GetSection("Jwt:ValidAudiences").Get<List<string>>()
                                 ?? throw new InvalidOperationException("'Audience' not found.");
            
            
            options.AddPolicy("CorsPolicy", policy =>
            {
                // policy.WithOrigins(origins.ToArray())
                // policy.WithOrigins("https://localhost:7036/")
                policy.WithOrigins("https://localhost:7061/")
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
   


    
}