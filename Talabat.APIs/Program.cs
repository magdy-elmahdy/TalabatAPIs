using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Helprer;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Reposotories.Centext;
using Talabat.Core.Services.Contract;
using Talabat.Core.Spacifications;
using Talabat.Repository;
using Talabat.Repository._Identity;
using Talabat.Repository.Data;
using Talabats.Application.AuthService;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            webApplicationBuilder.Services.AddControllers();
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });
            webApplicationBuilder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("IdentityConnection"));
            });
            webApplicationBuilder.Services.AddSingleton<IConnectionMultiplexer>( (serviceProvider)=>
            {
                var connection = webApplicationBuilder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            webApplicationBuilder.Services.AddScoped(typeof(IGenericReposotory<>), typeof(GenericReposotory<>));
            webApplicationBuilder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            webApplicationBuilder.Services.AddAutoMapper(typeof(MappigProfiles));
            webApplicationBuilder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
            {
                ApiBehaviorOptions.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors).Select(E=>E.ErrorMessage)
                    .ToList();

                    var Resposne = new ApiValidationErrorResponse(errors);
                    //var Resposne = new ApiValidationErrorResponse()
                    //{
                    //    Errors = errors

                    //};
                    return new BadRequestObjectResult(Resposne);
                };
            });

            webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>();
            webApplicationBuilder.Services.AddAuthentication().AddJwtBearer("Bearer",options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = webApplicationBuilder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = webApplicationBuilder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(webApplicationBuilder.Configuration["JWT:AuthKey"])),
                };
            });
            var app = webApplicationBuilder.Build();







            
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _storeContext = services.GetRequiredService<StoreContext>();//بطلب اوبجكت من StoreContext
            var _IdentiyDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();//بطلب اوبجكت من StoreContext
            var _loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _storeContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_storeContext);
                await _IdentiyDbContext.Database.MigrateAsync();
                var _usernager = services.GetRequiredService <UserManager<ApplicationUser>>();
                await ApplicationIdentityDataSeed.SeedUsersAsync(_usernager);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error has been occured during apply migration");
            }

            // Configure the HTTP request pipeline Middlewares.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }
}
