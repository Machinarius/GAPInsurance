using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Repositories;

namespace GAPInsurance.Domain.Services.Default {
  public class DefaultInsuranceService : IInsuranceService {
    private readonly IInsuranceDataRepository dataRepository;

    public DefaultInsuranceService(IInsuranceDataRepository dataRepository) {
      this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
    }

    public Task CreateClientAsync(string name) {
      throw new NotImplementedException();
    }

    public Task<InsurancePolicy> CreatePolicyAsync(string name, string description, Dictionary<InsuranceCoverage, float> coverages, DateTime coverageStartDate, 
                                                   int coverageLengthInMonths, float premiumPrice, RiskLevel riskLevel) {

      throw new NotImplementedException();
    }

    public Task DeleteClientAsync(Guid id) {
      throw new NotImplementedException();
    }

    public Task DeletePolicyAsync(Guid policyId) {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Client>> GetAllClientsAsync() {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync() {
      throw new NotImplementedException();
    }

    public Task<Client> GetClientAsync(Guid targetGuid) {
      throw new NotImplementedException();
    }

    public Task<InsurancePolicy> GetPolicyAsync(Guid policyId) {
      throw new NotImplementedException();
    }

    public Task UpdateClientAsync(Guid targetGuid) {
      throw new NotImplementedException();
    }

    public Task<InsurancePolicy> UpdatePolicyAsync(Guid policyId, string name, string description, float premiumPrice) {
      throw new NotImplementedException();
    }
  }
}
