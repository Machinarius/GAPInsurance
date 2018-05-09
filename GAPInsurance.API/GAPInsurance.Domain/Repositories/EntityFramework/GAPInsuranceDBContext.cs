using GAPInsurance.Domain.Repositories.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace GAPInsurance.Domain.Repositories.EntityFramework {
  public class GAPInsuranceDBContext : DbContext {
    public GAPInsuranceDBContext(DbContextOptions<GAPInsuranceDBContext> options) 
      : base(options) { }

    public virtual DbSet<DBClient> Clients { get; set; }
    public virtual DbSet<DBInsurancePolicy> Policies { get; set; }
    public virtual DbSet<DBClientCoverage> ClientCoverages { get; set; }

    protected override void OnModelCreating(ModelBuilder model) {
      base.OnModelCreating(model);

      model.Entity<DBClientCoverage>()
        .HasKey(cCoverage => new { cCoverage.PolicyId, cCoverage.ClientId });

      model.Entity<DBClientCoverage>()
        .HasOne(cCoverage => cCoverage.Policy)
        .WithMany(policy => policy.ClientCoverages)
        .HasForeignKey(cCoverage => cCoverage.PolicyId);

      model.Entity<DBClientCoverage>()
        .HasOne(cCoverage => cCoverage.Client)
        .WithMany(client => client.ClientCoverages)
        .HasForeignKey(cCoverage => cCoverage.ClientId);

      model.Entity<DBCoveragePercentage>()
        .HasKey(cPercentage => new { cPercentage.PolicyId, cPercentage.Coverage });

      model.Entity<DBCoveragePercentage>()
        .HasOne(cPercentage => cPercentage.Policy)
        .WithMany(policy => policy.CoveragePercentages)
        .HasForeignKey(cPercentage => cPercentage.PolicyId);
    }
  }

  public class GAPInsuranceDbContextFactory : IDesignTimeDbContextFactory<GAPInsuranceDBContext> {
    public GAPInsuranceDBContext CreateDbContext(string[] args) {
      var connectionString = Environment.GetEnvironmentVariable("GAP_DB");
      if (string.IsNullOrEmpty(connectionString)) {
        throw new InvalidOperationException("Cannot create the DBContext without a connection string in the 'GAP_DB' environment variable");
      }

      Console.WriteLine($"Using connection string: '{connectionString}'");
      var options = new DbContextOptionsBuilder<GAPInsuranceDBContext>()
        .UseSqlServer(connectionString)
        .Options;

      var dbContext = new GAPInsuranceDBContext(options);
      return dbContext;
    }
  }
}
