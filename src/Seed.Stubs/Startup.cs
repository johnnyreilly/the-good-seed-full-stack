using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Seed.Common.Logging;
using Seed.Common.Web.Api;
using ILogger = Serilog.ILogger;

namespace Seed.Stubs
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // Properties
        private IConfigurationRoot Configuration { get; }

        private IContainer ApplicationContainer { get; set; }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Use this method to add services to the container

            var logger = LoggingConfig.Configure(Configuration);
            var auditLogger = new AuditLogger(logger);
            Log.Logger = logger;

            Log.Logger.Information("Seed-Tools Stubs configuring services");

            services.AddMvc(config => { config.Filters.Add(new ServiceFilterAttribute(typeof(LogAttribute))); })
                .AddControllersAsServices()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddScoped<LogAttribute>();

            var builder = new ContainerBuilder();

            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterInstance(Configuration).As<IConfigurationRoot>();
            builder.RegisterInstance(auditLogger).As<IAuditLogger>();
            builder.RegisterModule(new LoggingModule(logger, auditLogger));

            var mapper = new MapperConfiguration(cfg => { cfg.AddProfiles(GetType().GetTypeInfo().Assembly); })
                .CreateMapper();

            builder.RegisterInstance(mapper);

            //
            // Add any container registrations here
            //

            builder.Populate(services);

            ApplicationContainer = builder.Build();
            var _config = ApplicationContainer.Resolve<IConfigurationRoot>();
            var x = _config.GetConnectionString("SeedToolsDataStoreConnection");
            //GlobalConfiguration.Configuration.UseSqlServerStorage(x);

            Log.Logger.Information("Seed-Tools Stubs configured services");

            var serviceProvider = new AutofacServiceProvider(ApplicationContainer);

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            //
            // Use this method to configure the HTTP request pipeline
            //

            Log.Logger.Information("Seed-Tools Stubs configuring application");

            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddSerilog();

            app.UseDeveloperExceptionPage();

            app.UseMvc();

            appLifetime.ApplicationStopped.Register(() =>
            {
                Log.CloseAndFlush();
                ApplicationContainer.Dispose();
            });

            Log.Logger.Information("Seed-Tools Stubs configured application");
        }
    }
}