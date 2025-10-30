using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helprer;
using Talabat.APIs.Middlewares;
using Talabat.Core.Reposotories.Centext;
using Talabat.Core.Spacifications;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped(typeof(IGenericReposotory<>), typeof(GenericReposotory<>));
            builder.Services.AddAutoMapper(typeof(MappigProfiles));

            builder.Services.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
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


            var app = builder.Build();

            
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _storeContext = services.GetRequiredService<StoreContext>();
            var _loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _storeContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_storeContext);
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
