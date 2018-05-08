using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GAPInsurance.Domain.Repositories {
  public interface IInsuranceDataRepository {
    Task StorePolicyAsync(InsurancePolicy insurancePolicy);
    Task<InsurancePolicy> GetPolicyAsync(Guid policyGuid);
    Task DeletePolicyAsync(Guid targetGuid);
    Task<IEnumerable<InsurancePolicy>> GetAllPoliciesAsync();
    Task DeleteClientAssignmentAsync(Guid policyId, Guid clientId);
    Task StoreClientAsync(Guid id, string name);
    Task<Client> GetClientAsync(Guid id);
    Task DeleteClientAsync(Guid id);
    Task<IEnumerable<Client>> GetAllClientsAsync();
  }
}
