using System;
using System.Collections.Generic;

namespace GAPInsurance.Domain.Models {
  public class InsurancePolicy {
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public IDictionary<InsuranceCoverage, float> CoveragePercentages { get; }
    public DateTimeOffset CoverageStartDate { get; }
    public int CoverageLengthInMonths { get; }
    public float PremiumCostInDollars { get; }
    public RiskLevel InsuredRiskLevel { get; }

    public InsurancePolicy(Guid id, string name, string description, IDictionary<InsuranceCoverage, float> coveragePercentages, 
                           DateTimeOffset coverageStartDate, int coverageLengthInMonths, float premiumCostInDollars, 
                           RiskLevel insuredRiskLevel) {
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Description = description ?? throw new ArgumentNullException(nameof(description));
      CoveragePercentages = coveragePercentages ?? throw new ArgumentNullException(nameof(coveragePercentages));
      CoverageStartDate = coverageStartDate;
      CoverageLengthInMonths = coverageLengthInMonths;
      PremiumCostInDollars = premiumCostInDollars;
      InsuredRiskLevel = insuredRiskLevel;
    }
  }
}
