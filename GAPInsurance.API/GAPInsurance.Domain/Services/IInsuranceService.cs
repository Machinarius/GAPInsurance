using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAPInsurance.Domain.Services {
  public interface IInsuranceService {
    Task<InsurancePolicy> GetPolicyAsync(Guid policyId);
    Task<InsurancePolicy> CreatePolicyAsync(string name, string description, Dictionary<InsuranceCoverage, float> coveragePercentages,
                                            DateTime coverageStartDate, int coverageLengthInMonths, float premiumPrice, RiskLevel riskLevel);
    Task DeletePolicyAsync(Guid policyId);
    Task UpdatePolicyAsync(string name, string description, float premiumPrice);
  }
}
