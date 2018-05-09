using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Repositories;
using GAPInsurance.Domain.Services;
using GAPInsurance.Domain.Services.Default;
using Moq;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
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

      repoMock
        .Setup(mock => mock.StorePolicyAsync(It.IsAny<InsurancePolicy>()))
        .Returns(Task.CompletedTask);

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

    [Fact]
    public async Task CreatingAValidPolicyMustBeStoredInTheRepository() {
      string name = "Test Policy";
      string description = "This is a test policy";
      Dictionary<InsuranceCoverage, float> coverages = new Dictionary<InsuranceCoverage, float> {
        { InsuranceCoverage.Theft, 50 },
        { InsuranceCoverage.Loss, 25 }
      };
      DateTime coverageStartDate = DateTime.Now;
      int coverageLengthInMonths = 10;
      float premiumPrice = 50;
      RiskLevel riskLevel = RiskLevel.Low;

      repoMock
        .Setup(mock => mock.StorePolicyAsync(It.Is<InsurancePolicy>(policy =>
          policy.Name == name &&
          policy.Description == description &&
          policy.CoveragePercentages.Count == coverages.Count &&
          !policy.CoveragePercentages.Except(coverages).Any() &&
          policy.CoverageStartDate == coverageStartDate &&
          policy.PremiumCostInDollars == premiumPrice &&
          policy.InsuredRiskLevel == riskLevel
        )))
        .Returns(Task.CompletedTask)
        .Verifiable();
      
      await service.CreatePolicyAsync(name, description, coverages, coverageStartDate, coverageLengthInMonths, premiumPrice, riskLevel);
      repoMock.Verify();
    }
    
    [Fact]
    public async Task RetrievingAPolicyMustCallThroughToTheRepository() {
      var policyGuid = Guid.NewGuid();
      var expectedPolicy = new InsurancePolicy(policyGuid, "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low);
      repoMock
        .Setup(mock => mock.GetPolicyAsync(policyGuid))
        .ReturnsAsync(expectedPolicy)
        .Verifiable();

      var actualPolicy = await service.GetPolicyAsync(policyGuid);
      Check.That(actualPolicy).IsSameReferenceAs(expectedPolicy);
      repoMock.Verify();
    }

    [Fact]
    public async Task UpdatingAPolicyMustCallThroughToTheRepositoryWithTheUpdatedValues() {
      var policyGuid = Guid.NewGuid();
      var originalPolicy = new InsurancePolicy(policyGuid, "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low);
      var expectedPolicy = new InsurancePolicy(policyGuid, "Updated Policy", "This is an updated policy", 
        new Dictionary<InsuranceCoverage, float>(), originalPolicy.CoverageStartDate, 10, 10, RiskLevel.Low);

      repoMock
        .Setup(mock => mock.GetPolicyAsync(policyGuid))
        .ReturnsAsync(originalPolicy);

      InsurancePolicy storedPolicy = null;
      repoMock
        .Setup(mock => mock.StorePolicyAsync(It.IsAny<InsurancePolicy>()))
        .Callback((InsurancePolicy policy) => { storedPolicy = policy; })
        .Returns(Task.CompletedTask)
        .Verifiable();

      var actualPolicy = await service.UpdatePolicyAsync(policyGuid, expectedPolicy.Name, expectedPolicy.Description, expectedPolicy.PremiumCostInDollars);
      Check.That(actualPolicy).IsNotNull();
      Check.That(actualPolicy).IsSameReferenceAs(storedPolicy);

      Check.That(expectedPolicy.Name).IsEqualTo(actualPolicy.Name);
      Check.That(expectedPolicy.Description).IsEqualTo(actualPolicy.Description);
      Check.That(expectedPolicy.CoveragePercentages).IsEqualTo(actualPolicy.CoveragePercentages);
      Check.That(expectedPolicy.CoverageStartDate).IsEqualTo(actualPolicy.CoverageStartDate);
      Check.That(expectedPolicy.PremiumCostInDollars).IsEqualTo(actualPolicy.PremiumCostInDollars);
      Check.That(expectedPolicy.InsuredRiskLevel).IsEqualTo(actualPolicy.InsuredRiskLevel);
    }

    [Fact]
    public async Task RetrievingAllPoliciesMustCallThroughToTheRepository() {
      var expectedPolicies = new List<InsurancePolicy> {
        new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low),
        new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low),
        new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low)
      };

      repoMock
        .Setup(mock => mock.GetAllPoliciesAsync())
        .ReturnsAsync(expectedPolicies.AsEnumerable());

      var actualPolicies = await service.GetAllPoliciesAsync();
      Check.That(actualPolicies).IsSameReferenceAs(expectedPolicies);
    }

    [Fact]
    public async Task DeletingAPolicyMustDeleteAllAssignmentsAndCallThroughToTheRepository() {
      var targetGuid = Guid.NewGuid();
      var client = new Client(Guid.NewGuid(), "Carlos");
      var policy = new InsurancePolicy(targetGuid, "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low, new[] { client });

      repoMock
        .Setup(mock => mock.GetPolicyAsync(policy.Id))
        .ReturnsAsync(policy)
        .Verifiable();

      repoMock
        .Setup(mock => mock.DeleteClientAssignmentAsync(policy.Id, client.Id))
        .Returns(Task.CompletedTask)
        .Verifiable();

      repoMock
        .Setup(mock => mock.DeletePolicyAsync(targetGuid))
        .Returns(Task.CompletedTask)
        .Verifiable();

      await service.DeletePolicyAsync(targetGuid);
      repoMock.Verify();
    }

    [Fact]
    public async Task CreatingAClientMustCallThroughToTheRepository() {
      repoMock
        .Setup(mock => mock.StoreClientAsync(It.IsAny<Guid>(), "Louise"))
        .Returns(Task.CompletedTask)
        .Verifiable();

      var actualClient = await service.CreateClientAsync("Louise");
      Check.That(actualClient).IsNotNull();
      Check.That(actualClient.Id).IsNotEqualTo(new Guid());
      Check.That(actualClient.Name).IsEqualTo("Louise");
      repoMock.Verify();
    }

    [Fact]
    public async Task DeletingAClientMustDeleteAllAssignmentsAndCallThroughToTheRepository() {
      var policy = new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low);
      var client = new Client(Guid.NewGuid(), "Carlos", new[] { policy });

      repoMock
        .Setup(mock => mock.GetClientAsync(client.Id))
        .ReturnsAsync(client);

      repoMock
        .Setup(mock => mock.DeleteClientAssignmentAsync(policy.Id, client.Id))
        .Returns(Task.CompletedTask);

      repoMock
        .Setup(mock => mock.DeleteClientAsync(client.Id))
        .Returns(Task.CompletedTask);

      await service.DeleteClientAsync(client.Id);
      repoMock.Verify();
    }

    [Fact]
    public async Task UpdatingAClientMustCallThroughToTheRepository() {
      var targetGuid = Guid.NewGuid();
      var client = new Client(targetGuid, "Alex");

      repoMock
        .Setup(mock => mock.GetClientAsync(targetGuid))
        .ReturnsAsync(client);

      repoMock
        .Setup(mock => mock.StoreClientAsync(targetGuid, "Louise"))
        .Returns(Task.CompletedTask)
        .Verifiable();

      await service.UpdateClientAsync(targetGuid, "Louise");
      repoMock.Verify();
    }

    [Fact]
    public async Task ReadingAClientMustCallThroughToTheRepository() {
      var targetGuid = Guid.NewGuid();
      var expectedClient = new Client(targetGuid, "Louise");

      repoMock
        .Setup(mock => mock.GetClientAsync(targetGuid))
        .ReturnsAsync(expectedClient);

      var actualClient = await service.GetClientAsync(targetGuid);
      Check.That(actualClient).IsSameReferenceAs(expectedClient);
    }

    [Fact]
    public async Task ReadingAllClientsMustCallThroughToTheRepository() {
      var expectedClients = new[] {
        new Client(Guid.NewGuid(), "Louise"),
        new Client(Guid.NewGuid(), "Louise"),
        new Client(Guid.NewGuid(), "Louise")
      };

      repoMock
        .Setup(mock => mock.GetAllClientsAsync())
        .ReturnsAsync(expectedClients);

      var actualClients = await service.GetAllClientsAsync();
      Check.That(actualClients).IsSameReferenceAs(actualClients);
    }

    [Fact]
    public async Task AssigningAPolicyToAClientMustCallThroughToTheRepository() {
      var clientId = Guid.NewGuid();
      var policyId = Guid.NewGuid();

      var client = new Client(Guid.NewGuid(), "Louise");
      var policy = new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low);

      repoMock
        .Setup(mock => mock.GetPolicyAsync(policyId))
        .ReturnsAsync(policy);

      repoMock
        .Setup(mock => mock.GetClientAsync(clientId))
        .ReturnsAsync(client);

      repoMock
        .Setup(mock => mock.AddClientAssignmentAsync(policyId, clientId))
        .Returns(Task.CompletedTask)
        .Verifiable();

      await service.AssignPolicyToClientAsync(policyId, clientId);
      repoMock.Verify();
    }

    [Fact]
    public async Task RemovingAPolicyFromAClientMustCallThroughToTheRepository() {
      var clientId = Guid.NewGuid();
      var policyId = Guid.NewGuid();

      var client = new Client(Guid.NewGuid(), "Louise");
      var policy = new InsurancePolicy(Guid.NewGuid(), "123", "123", new Dictionary<InsuranceCoverage, float>(), DateTime.Now, 10, 10, RiskLevel.Low, new[] { client });

      repoMock
        .Setup(mock => mock.GetPolicyAsync(policyId))
        .ReturnsAsync(policy);

      repoMock
        .Setup(mock => mock.GetClientAsync(clientId))
        .ReturnsAsync(client);

      repoMock
        .Setup(mock => mock.DeleteClientAssignmentAsync(policyId, clientId))
        .Returns(Task.CompletedTask)
        .Verifiable();

      await service.RemovePolicyFromClientAsync(policyId, clientId);
      repoMock.Verify();
    }
  }
}
