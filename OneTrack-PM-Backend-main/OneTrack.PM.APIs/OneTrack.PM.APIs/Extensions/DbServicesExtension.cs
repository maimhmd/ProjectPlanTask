using OneTrack.PM.Entities.Models.DB;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace OneTrack.PM.APIs.Extensions
{
    public static class DbServicesExtension
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OneTrackPMContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlDbConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            return services;
        }
    }
}
