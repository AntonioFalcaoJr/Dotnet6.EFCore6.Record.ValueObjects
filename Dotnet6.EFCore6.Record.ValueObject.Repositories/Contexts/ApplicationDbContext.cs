using Dotnet6.EFCore6.Record.ValueObject.Domain.Entities;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.Contexts.Extensions;
using Dotnet6.EFCore6.Record.ValueObject.Repositories.DependencyInjection.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private const string SqlLatin1GeneralCp1CsAs = "SQL_Latin1_General_CP1_CS_AS";
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly SqlServerRetryingOptions _options;

        public ApplicationDbContext(DbContextOptions options, ILoggerFactory loggerFactory, IConfiguration configuration, IOptionsSnapshot<SqlServerRetryingOptions> optionsSnapshot)
            : base(options) => (_loggerFactory, _configuration, _options) = (loggerFactory, configuration, optionsSnapshot.Value);

        public virtual DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation(SqlLatin1GeneralCp1CsAs);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Seed();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            optionsBuilder
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .UseSqlServer(
                    connectionString:_configuration.GetConnectionString("DefaultConnection"), 
                    sqlServerOptionsAction: SqlServerOptionsAction)
                .UseLoggerFactory(_loggerFactory);
        }

        private void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .ExecutionStrategy(
                    dependencies => new SqlServerRetryingExecutionStrategy(
                        dependencies: dependencies,
                        maxRetryCount: _options.MaxRetryCount,
                        maxRetryDelay: _options.MaxRetryDelay,
                        errorNumbersToAdd: _options.ErrorNumbersToAdd))
                .MigrationsAssembly(assemblyName: typeof(ApplicationDbContext).Assembly.GetName().Name);
    }
}