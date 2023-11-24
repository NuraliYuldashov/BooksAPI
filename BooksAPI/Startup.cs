using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Service;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BooksAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI;

public static class Startup
{
    readonly static string cors = "AllowAll";
    public static void AddDependencyInjectionServices(
        this WebApplicationBuilder builder)
    {
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Frontend uchun API ga ruxsat berish
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(cors,
                       builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                       });
        });

        // Add DB Context to DI Container
        builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

        builder.Services.AddDbContext<AuthDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));

        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddTransient<ICategoryInterface, CategoryRepository>();
        builder.Services.AddTransient<IBookInterface, BookRepository>();
        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<IUnitOfWorkInterface, UnitOfWork>();
        builder.Services.AddTransient<IBookService, BookService>();
        builder.Services.AddTransient<IUserService, UserService>();

        var key = Encoding.ASCII.GetBytes("NimadurMaxfiyKalit"); // Replace with a strong secret key
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        #region Add Automapper
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
        #endregion
        
    }

    public static void AddApplicationMiddelweares(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(cors);

        app.UseAuthorization();

        app.MapControllers();
        app.SeedRolesAndUsers().Wait();
        app.Run();
    }

    private static async Task SeedRolesAndUsers(this IApplicationBuilder builder)
    {
        using IServiceScope scope = builder.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roleNames = { "Admin", "User" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Create the roles and seed them to the database
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

    }
}
