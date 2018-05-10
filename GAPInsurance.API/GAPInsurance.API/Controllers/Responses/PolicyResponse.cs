using System;
using System.Collections.Generic;
using GAPInsurance.Domain.Models;

namespace GAPInsurance.API.Controllers.Responses {
  public class PolicyResponse {
    public string Name { get; set; }
    public string Description { get; set; }
    public float PremiumPrice { get; set; }
    public string CoverageStartDate { get; set; }
    public int CoverageLength { get; set; }
    public int RiskLevelId { get; set; }
    public float EarthquakeCoverage { get; set; }
    public float FireCoverage { get; set; }
    public float TheftCoverage { get; set; }
    public float LossCoverage { get; set; }
    public IEnumerable<string> CoveredClients { get; set; }

    public PolicyResponse() { }

    public PolicyResponse(InsurancePolicy model) {
      if (model == null) {
        throw new ArgumentNullException(nameof(model));
      }

      Name = model.Name;
      Description = model.Description;
      PremiumPrice = model.PremiumCostInDollars;
      CoverageStartDate = model.CoverageStartDate.ToShortDateString();
      CoverageLength = model.CoverageLengthInMonths;
      RiskLevelId = (int)model.InsuredRiskLevel;
      
      foreach (var percentage in model.CoveragePercentages) {
        switch(percentage.Key) {
          case InsuranceCoverage.Earthquake:
            EarthquakeCoverage = percentage.Value;
            break;
          case InsuranceCoverage.Fire:
            FireCoverage = percentage.Value;
            break;
          case InsuranceCoverage.Loss:
            LossCoverage = percentage.Value;
            break;
          case InsuranceCoverage.Theft:
            TheftCoverage = percentage.Value;
            break;
        }
      }
    }
  }
}

/*
export interface Policy {
  name: string,
  description: string,
  premiumPrice: number,
  coverageStartDate: string,
  coverageLength: number,
  riskLevelId: number,
  fireCoverage: number,
  theftCoverage: number,
  lossCoverage: number,
  coveredClients: string[]
}

 */