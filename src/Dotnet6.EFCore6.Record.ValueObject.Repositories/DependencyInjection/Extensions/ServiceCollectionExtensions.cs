using Dotnet6.EFCore6.Record.ValueObject.Repositories.Contexts;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
            => services
                .AddScoped<DbContext, ApplicationDbContext>()
                .AddDbContext<ApplicationDbContext>();

        public static OptionsBuilder<SqlServerRetryingOptions> ConfigureSqlServerRetryingOptions(this IServiceCollection services, IConfigurationSection section)
            => services
                .AddOptions<SqlServerRetryingOptions>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();
    }
}