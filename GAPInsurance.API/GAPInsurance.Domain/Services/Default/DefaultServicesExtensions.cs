using System;
using Microsoft.Extensions.DependencyInjection;

namespace GAPInsurance.Domain.Services.Default {
  public static class DefaultServicesExtensions {
    public static IServiceCollection AddDefaultBusinessServices(this IServiceCollection services) {
      if (services == null) {
        throw new ArgumentNullException(nameof(services));
      }

      return services.AddTransient<IInsuranceService, DefaultInsuranceService>();
    } 
  }
}