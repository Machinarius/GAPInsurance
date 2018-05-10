using System;
using System.Collections.Generic;
using System.Linq;
using GAPInsurance.Domain.Models;

namespace GAPInsurance.API.Controllers.Responses {
  public class PolicyResponse {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float PremiumPrice { get; set; }
    public string CoverageStartDate { get; set; }
    public int CoverageLength { get; set; }
    public string RiskLevel { get; set; }
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

      Id = model.Id.ToString();
      Name = model.Name;
      Description = model.Description;
      PremiumPrice = model.PremiumCostInDollars;
      CoverageStartDate = model.CoverageStartDate.ToShortDateString();
      CoverageLength = model.CoverageLengthInMonths;
      RiskLevel = model.InsuredRiskLevel.ToString();
      
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

      CoveredClients = model.CoveredClients.Select(client => client.Name).ToArray();
    }
  }
}
