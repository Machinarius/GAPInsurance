using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Models {
  public class DBInsurancePolicy {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CoverageStartDate { get; set; }
    public int CoverageLengthInMonths { get; set; }
    public float PremiumCostInDollars { get; set; }
    public RiskLevel InsuredRiskLevel { get; set; }
    public virtual ICollection<DBClientCoverage> ClientCoverages { get; set; }
    public virtual ICollection<DBCoveragePercentage> CoveragePercentages { get; set; }

    public DBInsurancePolicy() { }

    public DBInsurancePolicy(InsurancePolicy model) {
      Id = model.Id;
      Name = model.Name;
      Description = model.Description;
      CoverageStartDate = model.CoverageStartDate;
      CoverageLengthInMonths = model.CoverageLengthInMonths;
      PremiumCostInDollars = model.PremiumCostInDollars;
      InsuredRiskLevel = model.InsuredRiskLevel;
      // Assuming this constructor should only be used for _new_ policies without clients
      ClientCoverages = new List<DBClientCoverage>();
      CoveragePercentages = model.CoveragePercentages.Select(kvp => new DBCoveragePercentage {
        PolicyId = model.Id,
        Coverage = kvp.Key,
        Percentage = kvp.Value
      }).ToList();
    }

    public InsurancePolicy ToModel() {
      var percentagesDict = new Dictionary<InsuranceCoverage, float>();
      foreach (var percentage in CoveragePercentages) {
        percentagesDict[percentage.Coverage] = percentage.Percentage;
      }

      var coveredClients = ClientCoverages
        .Select(cCoverage => cCoverage.Client.ToModel())
        .ToList()
        .AsEnumerable();
      return new InsurancePolicy(Id, Name, Description, percentagesDict, CoverageStartDate, CoverageLengthInMonths, PremiumCostInDollars, InsuredRiskLevel, coveredClients);
    }

    internal void UpdateFrom(InsurancePolicy model) {
      if (model == null) {
        throw new ArgumentNullException(nameof(model));
      }

      Name = model.Name;
      Description = model.Description;
      CoverageStartDate = model.CoverageStartDate;
      CoverageLengthInMonths = model.CoverageLengthInMonths;
      PremiumCostInDollars = model.PremiumCostInDollars;
      InsuredRiskLevel = model.InsuredRiskLevel;
      // Ignoring clients

      var newPercentages = model.CoveragePercentages.Select(kvp => new DBCoveragePercentage {
        PolicyId = model.Id,
        Coverage = kvp.Key,
        Percentage = kvp.Value
      });

      CoveragePercentages.Clear();
      foreach (var percentage in newPercentages) {
        CoveragePercentages.Add(percentage);
      }
    }
  }
}

