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

    public Task<InsurancePolicy> CreatePolicyAsync(string name, string description, Dictionary<InsuranceCoverage, float> coverages, DateTime coverageStartDate, 
                                                   int coverageLengthInMonths, float premiumPrice, RiskLevel riskLevel) {

      throw new NotImplementedException();
    }

    public Task DeletePolicyAsync(Guid policyId) {
      throw new NotImplementedException();
    }

    public Task<InsurancePolicy> GetPolicyAsync(Guid policyId) {
      throw new NotImplementedException();
    }

    public Task UpdatePolicyAsync(string name, string description, float premiumPrice) {
      throw new NotImplementedException();
    }
  }
}
