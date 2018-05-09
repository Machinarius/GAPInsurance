using GAPInsurance.Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Models {
  [Table("PolicyCoverages")]
  public class DBCoveragePercentage {
    [Key]
    public Guid PolicyId { get; set; }
    [Key]
    public InsuranceCoverage Coverage { get; set; }
    public float Percentage { get; set; }

    public virtual DBInsurancePolicy Policy { get; set; }
  }
}
