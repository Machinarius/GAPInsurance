using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Repositories;
using GAPInsurance.Domain.Services;
using GAPInsurance.Domain.Services.Default;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GAPInsurance.Domain.Tests.Services {
  public class DefaultInsuranceServiceTests {
    private readonly IInsuranceService service;
    private readonly Mock<IInsuranceDataRepository> repoMock;

    public DefaultInsuranceServiceTests() {
      repoMock = new Mock<IInsuranceDataRepository>(MockBehavior.Strict);
      service = new DefaultInsuranceService(repoMock.Object);
    }

    [Fact]
    public void CreatingAPolicyWithDefaultValuesMustFail() {
      string name = null;
      string description = null;
      Dictionary<InsuranceCoverage, float> coverages = null;
      DateTime coverageStartDate = DateTime.Now;
      int coverageLengthInMonths = 0;
      float premiumPrice = 0;
      RiskLevel riskLevel = RiskLevel.None;

      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentNullException>();

      name = "Test policy";
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentNullException>();

      description = "This is a test";
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentNullException>();

      coverages = new Dictionary<InsuranceCoverage, float> {
        { InsuranceCoverage.Earthquake, 10 }
      };
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentOutOfRangeException>();

      coverageLengthInMonths = 10;
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentOutOfRangeException>();

      premiumPrice = 50;
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .Throws<ArgumentNullException>();

      riskLevel = RiskLevel.Low;
      Check
        .ThatAsyncCode(CreatePolicyAsync)
        .DoesNotThrow();

      async Task<InsurancePolicy> CreatePolicyAsync() {
        return await service.CreatePolicyAsync(name, description, coverages, coverageStartDate, coverageLengthInMonths, premiumPrice, riskLevel);
      }
    }

    [Fact]
    public void CreatingAPolicyWithoutCoveragesMustFail() {
      string name = "Test Policy";
      string description = "This is a test policy";
      Dictionary<InsuranceCoverage, float> coverages = new Dictionary<InsuranceCoverage, float>();
      DateTime coverageStartDate = DateTime.Now;
      int coverageLengthInMonths = 10;
      float premiumPrice = 50;
      RiskLevel riskLevel = RiskLevel.Low;

      Check.ThatAsyncCode(async () => {
        await service.CreatePolicyAsync(name, description, coverages, coverageStartDate, coverageLengthInMonths, premiumPrice, riskLevel);
      }).Throws<InvalidCoveragesException>();
    }

    [Fact]
    public void HighRiskPoliciesWith50OrHigherCoveragePercentMustBeConsideredInvalid() {
      string name = "Test Policy";
      string description = "This is a test policy";
      Dictionary<InsuranceCoverage, float> coverages = new Dictionary<InsuranceCoverage, float> {
        { InsuranceCoverage.Theft, 65 },
        { InsuranceCoverage.Loss, 25 }
      };
      DateTime coverageStartDate = DateTime.Now;
      int coverageLengthInMonths = 10;
      float premiumPrice = 50;
      RiskLevel riskLevel = RiskLevel.High;

      Check.ThatAsyncCode(async () => {
        await service.CreatePolicyAsync(name, description, coverages, coverageStartDate, coverageLengthInMonths, premiumPrice, riskLevel);
      }).Throws<InvalidCoveragesException>();
    }


  }
}
