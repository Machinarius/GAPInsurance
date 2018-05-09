using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAPInsurance.Domain.Services {
  public interface IInsuranceService {
    Task<InsurancePolicy> GetPolicyAsync(Guid policyId);
    Task<InsurancePolicy> CreatePolicyAsync(string name, string description, Dictionary<InsuranceCoverage, float> coveragePercentages,
                                            DateTime coverageStartDate, int coverageLengthInMonths, float premiumPrice, RiskLevel riskLevel);
    Task<InsurancePolicy> UpdatePolicyAsync(Guid policyId, string name, string description, float premiumPrice);
    Task DeletePolicyAsync(Guid policyId);
    Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync();
    Task<Client> CreateClientAsync(string name);
    Task DeleteClientAsync(Guid clientId);
    Task UpdateClientAsync(Guid clientId, string name);
    Task<Client> GetClientAsync(Guid clientId);
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task AssignPolicyToClientAsync(Guid policyId, Guid clientId);
    Task RemovePolicyFromClientAsync(Guid policyId, Guid clientId);
  }
}
