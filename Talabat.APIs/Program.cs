using Microsoft.EntityFrameworkCore;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
