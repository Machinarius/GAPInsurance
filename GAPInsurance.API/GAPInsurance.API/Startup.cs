using GAPInsurance.API.Configuration;
using GAPInsurance.Domain.Repositories.EntityFramework;
using GAPInsurance.Domain.Services.Default;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GAPInsurance.API {
  public class Startup {
    public Startup(IHostingEnvironment environment) {
      Configuration = new ConfigurationBuilder()
        .SetBasePath(environment.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json")
        .AddEnvironmentVariables()
        .Build();
      Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IHostingEnvironment Environment { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddMvc();

      var authConfigSection = Configuration.GetSection("Auth");
      var authConfig = authConfigSection.Get<AuthConfiguration>();

      services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options => {
        options.Authority = authConfig.Authority;
        options.Audience = authConfig.Audience;
      });

      services.AddDefaultBusinessServices();
      services.AddEntityFrameworkRepositories(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();
      app.UseMvc();
    }
  }
}
