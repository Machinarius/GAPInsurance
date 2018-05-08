using System;
using System.Collections.Generic;

namespace GAPInsurance.Domain.Models {
  public class Client {
    public Guid Id { get; }
    public string Name { get; }
    public IEnumerable<InsurancePolicy> AssignedPolicies { get; }

    public Client(Guid id, string name) {
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      AssignedPolicies = new InsurancePolicy[0];
    }

    public Client(Guid id, string name, IEnumerable<InsurancePolicy> assignedPolicies) {
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      AssignedPolicies = assignedPolicies ?? throw new ArgumentNullException(nameof(assignedPolicies));
    }
  }
}
