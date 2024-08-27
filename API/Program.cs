using API.DIConfiguration;
using API.Middleware;
using Domain.Models.ApplicationModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace API
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddLoki();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Enter JWT token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new List<string>()
                    }
                });
            });
            builder.ConfigureOptions();
            builder.ConfigureDatabases();
            builder.ConfigureRepositories();
            builder.ConfigureServices();            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("UserRole", "AdminRole"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("AdminRole"));
                options.AddPolicy("RequireHostRole", policy => policy.RequireRole("HostRole"));
                options.AddPolicy("RequireRestaurantRole", policy => policy.RequireRole("RestaurantRole", "HostRole"));
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Client,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,

                    };
                    options.SaveToken = true;
                }
            );
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }            
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseHttpsRedirection();
            app.MapControllers();
            using (IServiceScope scope = app.Services.CreateScope())
            {
                SQLContext context = scope.ServiceProvider.GetRequiredService<SQLContext>();
                context.Database.Migrate();
            }
            app.Run();
        }
    }
}
