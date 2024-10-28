using Microsoft.Extensions.DependencyInjection;
using OneTrack.PM.ActionFilters.FilterAttributeCreate;
using OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security;

namespace OneTrack.PM.APIs.Extensions
{
    public static class ActionFiltersServicesExtension
    {
        public static IServiceCollection AddActionFiltersServices(this IServiceCollection services)
        {
            #region Security
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateTitleActionExistsAttribute>();
            services.AddScoped<ValidateUserExistsAttribute>();
            services.AddScoped<ValidateGroupExistsAttribute>();
            services.AddScoped<ValidateContactExistsAttribute>();
            services.AddScoped<ValidateJobTitleExistsAttribute>();
            services.AddScoped<ValidateContactJobTitleExistsAttribute>();
            #endregion

            return services;
        }
    }
}
