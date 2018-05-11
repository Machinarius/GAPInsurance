using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GAPInsurance.API.Controllers.Responses {
  public class ClientResponse {
    public string Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<PolicyValue> AssignedPolicies { get; set; }

    public ClientResponse() { }

    public ClientResponse(Client model) {
      if (model == null) {
        throw new ArgumentNullException(nameof(model));
      }

      Id = model.Id.ToString();
      Name = model.Name;
      AssignedPolicies = model.AssignedPolicies.Select(policy => new PolicyValue(policy)).ToArray();
    }

    public class PolicyValue {
      public string Id { get; set; }
      public string Name { get; set; }

      public PolicyValue() { }

      public PolicyValue(InsurancePolicy policy) {
        if (policy == null) {
          throw new ArgumentNullException(nameof(policy));
        }

        Id = policy.Id.ToString();
        Name = policy.Name;
      }
    }
  }
}
