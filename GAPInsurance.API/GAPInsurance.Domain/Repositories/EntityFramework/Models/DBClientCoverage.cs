using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Models {
  [Table("ClientPolicies")]
  public class DBClientCoverage {
    public Guid ClientId { get; set; }
    public Guid PolicyId { get; set; }

    public DBClient Client { get; set; }
    public DBInsurancePolicy Policy { get; set; }

    public DBClientCoverage() { }

    public DBClientCoverage(Guid policyId, Guid clientId) {
      ClientId = clientId;
      PolicyId = policyId;
    }
  }
}
