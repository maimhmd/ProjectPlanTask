using Microsoft.Extensions.DependencyInjection;
using OneTrack.PM.APIs.Mapping;
using OneTrack.PM.APIs.Utility;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Repositories;
using OneTrack.PM.Services;

namespace OneTrack.PM.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOneTrackPMContextProcedures, OneTrackPMContextProcedures>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddAutoMapper(typeof(MappingProfiles));

            return services;
        }
    }
}
