using BLL;
using BLL.Services;
using BLL.Settings;
using DAL.data;
using DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure JWT settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddScoped<JwtService>();
       
            builder.Services.AddScoped<AuthService>();

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT SecretKey is not configured");
            }

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,

                            ValidIssuer = issuer,
                            ValidAudience = audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(secretKey)),

                            ClockSkew = TimeSpan.Zero
                        };
                });

            // Add services to the container.

            builder.Services.AddControllers().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter());
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactApp", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services for dependency injection
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
            builder.Services.AddScoped<IVacancyService, VacancyService>();

            builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
            builder.Services.AddScoped<IResumeService, ResumeService>();

            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();

            // Add AutoMapper
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();          

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("ReactApp");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}