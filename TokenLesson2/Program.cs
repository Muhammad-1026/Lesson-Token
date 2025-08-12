using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TokenLesson2.Common;
using TokenLesson2.Common.Mappings;
using TokenLesson2.DataContext;
using TokenLesson2.Interface.Repository;
using TokenLesson2.Interface.Services;
using TokenLesson2.Repositories;
using TokenLesson2.Services;

namespace TokenLesson2;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
        builder.Services.Configure<JwtSettings>(jwtSettingsSection);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IAuthRepository, AuthRepository>();
        builder.Services.AddTransient<IJwtRepository, JwtRepository>(); 

        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IJwtService, JwtService>();

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddDbContext<AplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}