using System.Text.Json.Serialization;
using Dotnet6.EFCore6.Record.ValueObject.Repositories;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Extensions;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Options;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.UnitsOfWork;
using Dotnet6.EFCore6.Record.ValueObject.Services;
using Dotnet6.EFCore6.Record.ValueObject.Services.Decorators;
using Dotnet6.EFCore6.Record.ValueObject.Services.MappingProfiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Dotnet6.EFCore6.Record.ValueObject.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder 
                => builder.AddSerilog());
            
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services
                .AddControllers()
                .AddJsonOptions(options 
                    => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
            
            services.AddMvcCore(
                options => options.SuppressAsyncSuffixInActionNames = false);

            services.AddAutoMapper(typeof(ModelToDomainProfile));
            
            services.AddApplicationDbContext();
            
            services.ConfigureSqlServerRetryingOptions(
                _configuration.GetSection(nameof(SqlServerRetryingOptions)));

            services.AddMemoryCache();
            
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPersonService, PersonService>();
            services.Decorate<IPersonService, PersonServiceLoggingDecorator>();
            services.Decorate<IPersonService, PersonServiceCacheDecorator>();
            
            services.AddSwaggerGen(options 
                => options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Dotnet6.EFCore6.Record.ValueObject.WebAPI", 
                    Version = "v1"
                }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options 
                    => options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json", 
                        name: "Dotnet6.EFCore6.Record.ValueObject.WebAPI v1"));
            }

            loggerFactory.AddSerilog();
            app.UseApiVersioning();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints 
                => endpoints.MapControllers());
        }
    }
}