using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Models {
  [Table("Policies")]
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

    public InsurancePolicy ToModel(bool includeClients) {
      var percentagesDict = new Dictionary<InsuranceCoverage, float>();
      foreach (var percentage in CoveragePercentages) {
        percentagesDict[percentage.Coverage] = percentage.Percentage;
      }

      IEnumerable<Client> coveredClients;
      if (includeClients) {
        coveredClients = ClientCoverages
          .Select(cCoverage => cCoverage.Client.ToModel(false))
          .ToList()
          .AsEnumerable();
      } else {
        coveredClients = new Client[0];
      }
      
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

      var newCoveragesList = new List<KeyValuePair<InsuranceCoverage, float>>(model.CoveragePercentages);
      var coveragesToDelete = new List<InsuranceCoverage>();
      foreach (var oldCoverage in CoveragePercentages) {
        KeyValuePair<InsuranceCoverage, float> matchingCoverage;
        try {
          matchingCoverage = newCoveragesList.First(x => x.Key == oldCoverage.Coverage);
        } catch (InvalidOperationException) {
          coveragesToDelete.Add(oldCoverage.Coverage);
          continue;
        }

        oldCoverage.Percentage = matchingCoverage.Value;
        newCoveragesList.Remove(matchingCoverage);
      }

      foreach (var deletedCoverage in coveragesToDelete) {
        var matchingCoverage = CoveragePercentages.FirstOrDefault(x => x.Coverage == deletedCoverage);
        if (matchingCoverage != null) {
          CoveragePercentages.Remove(matchingCoverage);
        }
      }

      foreach (var newCoverage in newCoveragesList) {
        CoveragePercentages.Add(new DBCoveragePercentage {
          Coverage = newCoverage.Key,
          Percentage = newCoverage.Value
        });
      }
    }
  }
}

