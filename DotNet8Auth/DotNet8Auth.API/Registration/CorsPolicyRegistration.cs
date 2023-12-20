namespace DotNet8Auth.API.Registration;

public static class CorsPolicyRegistration
{
    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.WithOrigins("https://localhost:7036/")
                    .SetIsOriginAllowed((_) => true)
                    // .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }


    
}