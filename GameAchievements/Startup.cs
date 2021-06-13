using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAchievements.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using System.IO;
using GameAchievements.LoggerService;
using GameAchievements.ActionFilters;
using GameAchievements.RequestFeatures.Extensions.DataShaper;
using GameAchievements.Models.DataTransferObjects;
using GameAchievements.RequestFeatures.Extensions.DataShaper.Impl;
using GameAchievements.Models.Authentication;
using GameAchievements.Models.Authentication.Impl;

namespace GameAchievements
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(),
                "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureREpositoryManager();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlSerializerFormatters();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GameAchievements", Version = "v1" });
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateGameExistsAttribute>();
            services.AddScoped<ValidateGenreExistsAttribute>();
            services.AddScoped<ValidateAchievementExistsAttribute>();
            services.AddScoped<IDataShaper<GameDto>, DataShaper<GameDto>>();
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameAchievements v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
