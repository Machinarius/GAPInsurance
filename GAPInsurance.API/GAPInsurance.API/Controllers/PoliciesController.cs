using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAPInsurance.API.Controllers.Requests;
using GAPInsurance.API.Controllers.Responses;
using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GAPInsurance.API.Controllers {
  [Authorize]
  [Route("api/Policies")]
  public class PoliciesController: Controller {
    private readonly IInsuranceService insuranceService;

    public PoliciesController(IInsuranceService insuranceService) {
      this.insuranceService = insuranceService ?? throw new ArgumentNullException(nameof(insuranceService));
    }

    [HttpGet]
    public async Task<IEnumerable<PolicyResponse>> GetPolicies() {
      var policies = await insuranceService.GetAllPoliciesAsync();
      var response = policies.Select(model => new PolicyResponse(model)).ToArray();

      return response;
    }

    [HttpDelete]
    [Route("{policyIdString}")]
    public async Task<IActionResult> DeletePolicy([FromRoute] string policyIdString) {
      Guid policyId;
      try {
        policyId = Guid.Parse(policyIdString);
      } catch (FormatException) {
        return BadRequest();
      }

      try {
        await insuranceService.DeletePolicyAsync(policyId);
        // Trying to delete a policy that does not exist should not fail
      } catch (ResourceNotFoundException) { }

      return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePolicy([FromBody] PolicyCreationRequest request) {
      if (string.IsNullOrEmpty(request.Name) ||
          string.IsNullOrEmpty(request.Description) ||
          string.IsNullOrEmpty(request.CoverageStartDate) ||
          request.CoverageLength <= 0 ||
          request.PremiumPrice <= 0 ||
          !Enum.IsDefined(typeof(RiskLevel), request.RiskLevelId)) {
        return BadRequest();
      }

      var coverages = new Dictionary<InsuranceCoverage, float>();
      if (request.EarthquakeCoverage > 0) {
        coverages.Add(InsuranceCoverage.Earthquake, request.EarthquakeCoverage);
      }

      if (request.FireCoverage > 0) {
        coverages.Add(InsuranceCoverage.Fire, request.FireCoverage);
      }

      if (request.TheftCoverage > 0) {
        coverages.Add(InsuranceCoverage.Theft, request.TheftCoverage);
      }

      if (request.LossCoverage > 0) {
        coverages.Add(InsuranceCoverage.Loss, request.LossCoverage);
      }

      if (!coverages.Any()) {
        return BadRequest();
      }

      DateTime coverageStartDate;
      try {
        coverageStartDate = DateTime.Parse(request.CoverageStartDate);
      } catch (FormatException) {
        return BadRequest();
      }

      var riskLevel = (RiskLevel)request.RiskLevelId;
      var policy = await insuranceService.CreatePolicyAsync(
        request.Name, request.Description, coverages, coverageStartDate, 
        request.CoverageLength, request.PremiumPrice, riskLevel);

      var response = new PolicyResponse(policy);
      return Ok(response);
    }

    [HttpPut]
    [Route("{policyIdString}")]
    public async Task<IActionResult> UpdatePolicy([FromRoute] string policyIdString, [FromBody] PolicyCreationRequest request) {
      if (string.IsNullOrEmpty(policyIdString) ||
          string.IsNullOrEmpty(request.Name) ||
          string.IsNullOrEmpty(request.Description) ||
          string.IsNullOrEmpty(request.CoverageStartDate) ||
          request.CoverageLength <= 0 ||
          request.PremiumPrice <= 0 ||
          !Enum.IsDefined(typeof(RiskLevel), request.RiskLevelId)) {
        return BadRequest();
      }

      Guid policyId;
      try {
        policyId = Guid.Parse(policyIdString);
      } catch (FormatException) {
        return BadRequest();
      }

      var coverages = new Dictionary<InsuranceCoverage, float>();
      if (request.EarthquakeCoverage > 0) {
        coverages.Add(InsuranceCoverage.Earthquake, request.EarthquakeCoverage);
      }

      if (request.FireCoverage > 0) {
        coverages.Add(InsuranceCoverage.Fire, request.FireCoverage);
      }

      if (request.TheftCoverage > 0) {
        coverages.Add(InsuranceCoverage.Theft, request.TheftCoverage);
      }

      if (request.LossCoverage > 0) {
        coverages.Add(InsuranceCoverage.Loss, request.LossCoverage);
      }

      if (!coverages.Any()) {
        return BadRequest();
      }

      DateTime coverageStartDate;
      try {
        coverageStartDate = DateTime.Parse(request.CoverageStartDate);
      } catch (FormatException) {
        return BadRequest();
      }

      var riskLevel = (RiskLevel)request.RiskLevelId;

      InsurancePolicy policy;
      try {
        policy = await insuranceService.GetPolicyAsync(policyId);
      } catch (ResourceNotFoundException) {
        return NotFound();
      }

      var updatedPolicy = await insuranceService.UpdatePolicyAsync(policyId, request.Name, request.Description, request.PremiumPrice);
      return Ok(updatedPolicy);
    }
  }
}