using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Repositories;

namespace GAPInsurance.Domain.Services.Default {
  public class DefaultInsuranceService : IInsuranceService {
    private readonly IInsuranceDataRepository dataRepository;

    public DefaultInsuranceService(IInsuranceDataRepository dataRepository) {
      this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
    }

    public async Task<Client> CreateClientAsync(string name) {
      if (string.IsNullOrEmpty(name)) {
        throw new ArgumentNullException(nameof(name));
      }

      var clientId = Guid.NewGuid();
      var client = new Client(clientId, name);
      await dataRepository.StoreClientAsync(clientId, name);

      return client;
    }

    public async Task<InsurancePolicy> CreatePolicyAsync(string name, string description, Dictionary<InsuranceCoverage, float> coverages, DateTime coverageStartDate, 
                                                   int coverageLengthInMonths, float premiumPrice, RiskLevel riskLevel) {
      if (string.IsNullOrEmpty(name)) {
        throw new ArgumentNullException(nameof(name));
      }

      if (string.IsNullOrEmpty(description)) {
        throw new ArgumentNullException(nameof(description));
      }

      if (coverages == null) {
        throw new ArgumentNullException(nameof(coverages));
      }

      if (coverageLengthInMonths <= 0) {
        throw new ArgumentOutOfRangeException(nameof(coverageLengthInMonths), "Coverage must at least last for a month");
      }

      if (premiumPrice <= 0) {
        throw new ArgumentOutOfRangeException(nameof(premiumPrice), "Coverage cannot be priced at 0 dollars");
      }

      if (!coverages.Any()) {
        throw new InvalidOperationException("Policies must have at least one coverage");
      }

      if (coverages.Any(coverage => coverage.Value < 0)) {
        throw new InvalidOperationException("Coverages must be effective for more than 0% of the insured value");
      }

      if (coverages.Any(coverage => coverage.Key == InsuranceCoverage.None)) {
        throw new InvalidCoveragesException("Cannot register a coverage for an invalid coverage type");
      }

      if (riskLevel == RiskLevel.High) {
        if (coverages.Any(coverage => coverage.Value > 50)) {
          throw new InvalidCoveragesException("High risk policies cannot cover for more than 50% of the insured value");
        }
      }

      var policy = new InsurancePolicy(Guid.NewGuid(), name, description, coverages, coverageStartDate, coverageLengthInMonths, premiumPrice, riskLevel);
      await dataRepository.StorePolicyAsync(policy);

      return policy;
    }

    public async Task DeleteClientAsync(Guid clientId) {
      Client client;
      try {
        client = await dataRepository.GetClientAsync(clientId);
      } catch (ResourceNotFoundException) {
        return;
      }

      foreach (var policy in client.AssignedPolicies) {
        await dataRepository.DeleteClientAssignmentAsync(policy.Id, client.Id);
      }

      await dataRepository.DeleteClientAsync(clientId);
    }

    public async Task DeletePolicyAsync(Guid policyId) {
      InsurancePolicy policy;
      try {
        policy = await dataRepository.GetPolicyAsync(policyId);
      } catch (ResourceNotFoundException) {
        return;
      }

      foreach (var client in policy.CoveredClients) {
        await dataRepository.DeleteClientAssignmentAsync(policy.Id, client.Id);
      }

      await dataRepository.DeletePolicyAsync(policyId);
    }

    public Task<IEnumerable<Client>> GetAllClientsAsync() {
      return dataRepository.GetAllClientsAsync();
    }

    public Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync() {
      return dataRepository.GetAllPoliciesAsync();
    }

    public Task<Client> GetClientAsync(Guid clientId) {
      return dataRepository.GetClientAsync(clientId);
    }

    public Task<InsurancePolicy> GetPolicyAsync(Guid policyId) {
      return dataRepository.GetPolicyAsync(policyId);
    }

    public async Task UpdateClientAsync(Guid clientId, string name) {
      if (string.IsNullOrEmpty(name)) {
        throw new ArgumentNullException(nameof(name));
      }
      
      var client = await dataRepository.GetClientAsync(clientId);
      await dataRepository.StoreClientAsync(clientId, name);
    }

    public async Task<InsurancePolicy> UpdatePolicyAsync(Guid policyId, string name, string description, float premiumPrice) {
      if (string.IsNullOrEmpty(name)) {
        throw new ArgumentNullException(nameof(name));
      }

      if (string.IsNullOrEmpty(description)) {
        throw new ArgumentNullException(nameof(description));
      }

      if (premiumPrice <= 0) {
        throw new ArgumentOutOfRangeException(nameof(premiumPrice), "Coverage cannot be priced at 0 dollars");
      }

      var policy = await dataRepository.GetPolicyAsync(policyId);
      var updatedPolicy = new InsurancePolicy(policyId, name, description, policy.CoveragePercentages, policy.CoverageStartDate, 
        policy.CoverageLengthInMonths, policy.PremiumCostInDollars, policy.InsuredRiskLevel, policy.CoveredClients);

      throw new NotImplementedException();
    }
  }
}
