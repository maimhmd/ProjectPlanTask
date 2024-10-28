using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NLog;
using OneTrack.PM.APIs.Extensions;
using OneTrack.PM.APIs.Middlewares;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Repositories;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Repositories;
using OneTrack.PM.Services;
using Microsoft.OpenApi.Models; 

namespace OneTrack.PM.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            #region Configure Services
            builder.Services.AddScoped<ILoggerService, LoggerService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProjectPlanService, ProjectPlanService>();
            builder.Services.AddScoped<IProjectPlanRepository, ProjectPlanRepository>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddActionFiltersServices();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            builder.Services.AddDbContext<OneTrackPMContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OneTrackPMDatabase")));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SLA-Api", Version = "v1" });
            });
            #endregion

            var app = builder.Build();

            #region Configure App [Kestrel] Middlewares
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SLA-Api v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
                RequestPath = "/Resources"
            });

            app.UseCors("MyPolicy");

            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}
