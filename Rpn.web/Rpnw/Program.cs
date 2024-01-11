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


            // REGISTER SWAGGER GENERATOR
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "RPN API", Version = "v1" });
                c.EnableAnnotations();
                c.SchemaFilter<SwaggerEnumValuesFilter>();
                c.OperationFilter<SwaggerDefaultValues>();
            });


            // CONFIGURE THE REPONSE FORMAT - API
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            });

            // Register the Versioning Services
            AddApiVersioningConfigured(builder.Services);



            // REGISTER HEALTH CHECKER
            builder.Services.AddHealthChecks().AddCheck<HealthCheckerService>(nameof(HealthCheckerService));

            // REGISTER AUTOMAPPER
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // REGISTER CACHE SERCICEE
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICache, MemoryCache>();

            // REGISTER DOMAIN SERCICEE
            //builder.Services.AddScoped<IIndicatorService, IndicatorService>();
            //builder.Services.AddScoped<IMeasureService, MeasureService>();


            // REGISTER INFRA SERCICEE - For REPOSITORY TO PERSIST STATE
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(DapperGenericRepository<>));


            // REGISTER ALL CALCULATOR SERVICE
            builder.Services.AddSingleton<IRpnStackFactory, RpnStackFactory>();
            // pour le moment c'est un sigleton l'etat est partagé entre tous les utilisateur, il n'est pas isolé et surtout pas thread safe
            // TODO: passer en mode thread safe de l'etat
            builder.Services.AddSingleton<IRpnCalculator, RpnCalculator>();
            builder.Services.AddScoped<IStackService, StackService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();

            // REGISTER SQL DATABASE - FOR TEST USING SQL LITE
            SQLitePCL.Batteries.Init();
            builder.Services.AddScoped<IDbConnection>((sp) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new SqliteConnection(configuration.GetConnectionString("SQLiteConnection"));
            });


            // ADD CERTIFICATE
            builder.Services.AddAuthentication( CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            if (app.Environment.IsDevelopment())
            {
                // ADD SWAGGER END POINT
                app.UseSwagger();

                // ADD SWAGGER UI FOR OUR SWAGGER END POINT
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("v1/swagger.json", "RPN Api");
                //});

                // ADD SWAGGER FOR VERSIONNING
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName;
                        options.SwaggerEndpoint(url, name);
                    }
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");


            app.MapHealthChecks("/health", new()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });


            app.MapFallbackToFile("index.html");

            app.Run();

        }


        public static void AddApiVersioningConfigured(IServiceCollection services)
        {


            var apiVersioningBuilder = services.AddEndpointsApiExplorer().AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                //o.ApiVersionReader = ApiVersionReader.Combine(
                  //  new UrlSegmentApiVersionReader()
                    //,
                   // new HeaderApiVersionReader("x-api-version"),
                    //new MediaTypeApiVersionReader("x-api-version")
                    //);
            });

            apiVersioningBuilder.AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });


        }
    }

}

  
