using Microsoft.AspNetCore.Http.Json;
using Rpnw.Domain.Impl.Services;
using Rpnw.Domain.Services;
using Rpnw.Infrastructure.Impl.Repository;
using Rpnw.Infrastructure.Repository;
using System;
using System.Collections.Concurrent;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Rpnw.Infrastructure.Cache;
using Rpnw.Infrastructure.Impl.Cache;
using Microsoft.Win32;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.OpenApi.Models;
using Rpnw.Presentation.Rest.Services.Attributs;
using Rpnw.Swagger;
using Newtonsoft.Json.Serialization;
using Rpnw.Domain.Impl.Rpn.Calculator;
using Rpnw.Domain.Model.Rpn;
using Rpnw.Domain.Impl.Rpn.Stack;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using HealthChecks.UI.Client;

namespace Rpnw
{

    class Program
    {
        static void Main(string[] args)
        {
            RunWebApiApplication(args);
        }

        static void RunWebApiApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            ConfigureSwagger(builder);
            var app = builder.Build();
            ConfigureApp(app);
            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                });

            AddApiVersioningConfigured(builder.Services);
            builder.Services.AddHealthChecks().AddCheck<HealthCheckerService>(nameof(HealthCheckerService));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICache, MemoryCache>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(DapperGenericRepository<>));
            builder.Services.AddSingleton<IRpnStackFactory, RpnStackFactory>();
            builder.Services.AddSingleton<IRpnCalculator, RpnCalculator>();
            builder.Services.AddScoped<IStackService, StackService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();
            ConfigureDatabase(builder);
        }

        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SchemaFilter<SwaggerEnumValuesFilter>();
                c.OperationFilter<SwaggerDefaultValues>();
            });
        }

        private static void ConfigureApp(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        options.SwaggerEndpoint(url, description.GroupName);
                    }
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
            app.MapHealthChecks("/health", new() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
            app.MapFallbackToFile("index.html");
        }

        private static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            SQLitePCL.Batteries.Init();
            builder.Services.AddScoped<IDbConnection>((sp) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new SqliteConnection(configuration.GetConnectionString("SQLiteConnection"));
            });
        }

        public static void AddApiVersioningConfigured(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                .AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }


}

  
