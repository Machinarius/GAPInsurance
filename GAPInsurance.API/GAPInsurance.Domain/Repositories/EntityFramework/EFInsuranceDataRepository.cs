using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Repositories.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace GAPInsurance.Domain.Repositories.EntityFramework {
  public class EFInsuranceDataRepository : IInsuranceDataRepository {
    private readonly GAPInsuranceDBContext dbContext;

    public EFInsuranceDataRepository(GAPInsuranceDBContext dbContext) {
      this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddClientAssignmentAsync(Guid policyId, Guid clientId) {
      var cCoverage = await dbContext.ClientCoverages.FindAsync(policyId, clientId);
      if (cCoverage != null) {
        return;
      }

      cCoverage = new DBClientCoverage(policyId, clientId);
      dbContext.ClientCoverages.Add(cCoverage);
      await dbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAssignmentAsync(Guid policyId, Guid clientId) {
      var cCoverage = await dbContext.ClientCoverages.FindAsync(policyId, clientId);
      if (cCoverage == null) {
        return;
      }

      dbContext.Remove(cCoverage);
      await dbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(Guid clientId) {
      var client = await dbContext.Clients.FindAsync(clientId);
      if (client == null) {
        return;
      }

      dbContext.Remove(client);
      await dbContext.SaveChangesAsync();
    }

    public async Task DeletePolicyAsync(Guid policyId) {
      var policy = await dbContext.Policies.FindAsync(policyId);
      if (policy == null) {
        return;
      }

      dbContext.Remove(policy);
      await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync() {
      var dbClients = await dbContext.Clients
        .Include(client => client.ClientCoverages)
          .ThenInclude(coverage => coverage.Policy)
            .ThenInclude(policy => policy.CoveragePercentages)
        .ToListAsync();
      var clients = dbClients.Select(dbClient => dbClient.ToModel(true)).ToArray();

      return clients.AsEnumerable();
    }

    public async Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync() {
      var dbPolicies = await dbContext.Policies
        .Include(policy => policy.ClientCoverages)
          .ThenInclude(coverage => coverage.Client)
        .Include(policy => policy.CoveragePercentages)
        .ToListAsync();
      var policies = dbPolicies.Select(dbPolicy => dbPolicy.ToModel(true)).ToArray();

      return policies.AsEnumerable();
    }

    public async Task<Client> GetClientAsync(Guid clientId) {
      var dbClient = await dbContext.Clients
        .Include(_client => _client.ClientCoverages)
          .ThenInclude(_coverage => _coverage.Policy)
            .ThenInclude(_policy => _policy.CoveragePercentages)
        .FirstOrDefaultAsync(_client => _client.Id == clientId);
      if (dbClient == null) {
        throw new ResourceNotFoundException($"Could not find a client for id {clientId}");
      }

      var client = dbClient.ToModel(true);
      return client;
    }

    public async Task<InsurancePolicy> GetPolicyAsync(Guid policyId) {
      var dbPolicy = await dbContext.Policies
        .Include(_policy => _policy.ClientCoverages)
        .Include(_policy => _policy.CoveragePercentages)
        .FirstOrDefaultAsync(_policy => _policy.Id == policyId);
      if (dbPolicy == null) {
        throw new ResourceNotFoundException($"Could not find a policy for id {policyId}");
      }

      var policy = dbPolicy.ToModel(true);
      return policy;
    }

    public async Task StoreClientAsync(Guid id, string name) {
      var dbClient = await dbContext.Clients.FindAsync(id);
      if (dbClient == null) {
        dbClient = new DBClient {
          Id = id,
          Name = name
        };

        dbContext.Clients.Add(dbClient);
      } else {
        dbClient.Name = name;
      }

      await dbContext.SaveChangesAsync();
    }

    public async Task StorePolicyAsync(InsurancePolicy insurancePolicy) {
      if (insurancePolicy == null) {
        throw new ArgumentNullException(nameof(insurancePolicy));
      }

      var dbPolicy = await dbContext.Policies.FindAsync(insurancePolicy.Id);
      if (dbPolicy == null) {
        dbPolicy = new DBInsurancePolicy(insurancePolicy);
        dbContext.Policies.Add(dbPolicy);
      } else {
        dbPolicy.UpdateFrom(insurancePolicy);
      }

      await dbContext.SaveChangesAsync();
    }
  }
}
