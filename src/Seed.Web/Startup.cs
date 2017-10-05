using System;
using System.ComponentModel;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Seed.Api.Controllers;
using Seed.Api.Infrastructure;
using Seed.Common.Autofac;
using Seed.Common.AutoMapper;
using Seed.Common.Logging;
using Seed.Common.Web.Api;
using Seed.Data;
using Seed.Data.Infrastructure;
using Seed.Domain.Infrastructure;
using Seed.Domain.Infrastructure.Configuration;
using IContainer = Autofac.IContainer;
using ILogger = Serilog.ILogger;

namespace Seed.Web
{
    public class Startup
    {
        // C'tor
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // Properties
        private IConfigurationRoot Configuration { get; }

        private IContainer ApplicationContainer { get; set; }


        // Runtime Hooks
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //
            // Use this method to add services to the container
            //

            var logger = ConfigureLogging(Configuration);
            var auditLogger = new AuditLogger(logger);
            Log.Logger = logger;

            Log.Logger.Information("Seed-Tools configuring services");

            services.AddOptions();
            services.AddCors();
            services.Configure<TpdConfiguration>(Configuration.GetSection("Domain:Tpd"));
            services.Configure<NonWorkingDaysConfiguration>(Configuration.GetSection("Domain:NonWorkingDays"));
            services.Configure<SecurityConfiguration>(Configuration.GetSection("Domain:Security"));

            ConfigureDatabase(services);

            services.AddHangfire(x =>
                x.UseSqlServerStorage(Configuration.GetConnectionString("SeedToolsDataStoreConnection")));

            services.AddMvc(config =>
                {
                    config.Filters.Add(new ServiceFilterAttribute(typeof(LogAttribute)));
                    config.Filters.Add(new ServiceFilterAttribute(typeof(TransformApiResultAttribute)));
                    config.Filters.Add(typeof(ValidateModelBindingsAttribute), 2);
                })
                .AddApplicationPart(typeof(StatusController).GetTypeInfo().Assembly)
                .AddControllersAsServices()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddScoped<LogAttribute>();
            services.AddScoped<TransformApiResultAttribute>();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterInstance(auditLogger).As<IAuditLogger>();
            builder.RegisterModule(new LoggingModule(logger, auditLogger));
            builder.RegisterInstance(Configuration).As<IConfigurationRoot>();

            var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles<ApiRegistry>();
                    cfg.AddProfiles<DomainRegistry>();
                    cfg.AddProfiles<DataRegistry>();
                })
                .CreateMapper();

            builder.RegisterInstance(mapper);

            ConfigureContainer(builder, services);

            builder.Populate(services);

            ApplicationContainer = builder.Build();
            
            Log.Logger.Information("Seed-Tools configured services");

            var serviceProvider = new AutofacServiceProvider(ApplicationContainer);

            return serviceProvider;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            //
            // Use this method to configure the HTTP request pipeline
            //

            Log.Logger.Information("Seed-Tools configuring application");

            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddSerilog();

            ConfigureHangfireServer(app);

            app.UseFileServer();

            if (env.IsDevelopment())
            {
                Log.Logger.Information("In Development");
                app.UseDeveloperExceptionPage();
                app.UseHangfireDashboard();
                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            }

            app.UseMiddleware<GcnHeaderSecurityMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute("spa-fallback", new {controller = "Spa", action = "Index"});
            });

            appLifetime.ApplicationStopped.Register(() =>
            {
                Log.CloseAndFlush();
                ApplicationContainer.Dispose();
            });

            Log.Logger.Information("Seed-Tools configured application");
        }


        // Virtual Members
        protected virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SeedToolsDbContext>(opt =>
                {
                    opt.UseSqlServer(Configuration.GetConnectionString("SeedToolsDataStoreConnection"));
                });
        }

        protected virtual ILogger ConfigureLogging(IConfigurationRoot configuration)
        {
            return LoggingConfig.Configure(configuration);
        }

        protected virtual void ConfigureHangfireServer(IApplicationBuilder app)
        {
            app.UseHangfireServer();
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder, IServiceCollection services)
        {
            builder.IncludeRegistry<ApiRegistry>();
            builder.IncludeRegistry<DomainRegistry>();
            builder.IncludeRegistry<DataRegistry>();
        }
    }
}