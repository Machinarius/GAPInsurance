using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GAPInsurance.Domain.Repositories.EntityFramework {
  public static class EFRepositoriesExtensions {
    public const string ConnectionStringName = "GAPInsuranceDB";

    public static IServiceCollection AddEntityFrameworkRepositories(this IServiceCollection services, IConfiguration configuration) {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      if (configuration == null) {
        throw new ArgumentNullException(nameof(configuration));
      }

      var connectionString = configuration.GetConnectionString(ConnectionStringName);
      if (string.IsNullOrEmpty(connectionString)) {
        throw new InvalidOperationException($"Cannot add EF repositories without a '{ConnectionStringName}' connection string");
      }

      return services
        .AddDbContext<GAPInsuranceDBContext>(options => options.UseSqlServer(connectionString))
        .AddTransient<IInsuranceDataRepository, EFInsuranceDataRepository>();
    }
  }
}