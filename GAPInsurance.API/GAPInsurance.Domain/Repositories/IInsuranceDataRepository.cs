using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAPInsurance.Domain.Repositories {
  public interface IInsuranceDataRepository {
    Task StorePolicyAsync(InsurancePolicy insurancePolicy);
    Task<InsurancePolicy> GetPolicyAsync(Guid policyId);
    Task DeletePolicyAsync(Guid policyId);
    Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync();
    Task DeleteClientAssignmentAsync(Guid policyId, Guid clientId);
    Task StoreClientAsync(Guid id, string name);
    Task<Client> GetClientAsync(Guid clientId);
    Task DeleteClientAsync(Guid clientId);
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task AddClientAssignmentAsync(Guid policyId, Guid clientId);
  }
}
