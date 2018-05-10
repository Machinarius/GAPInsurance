using GAPInsurance.API.Controllers.Requests;
using GAPInsurance.API.Controllers.Responses;
using GAPInsurance.Domain.Exceptions;
using GAPInsurance.Domain.Models;
using GAPInsurance.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPInsurance.API.Controllers {
  [Authorize]
  [Route("api/Clients")]
  public class ClientsController : Controller {
    private readonly IInsuranceService insuranceService;

    public ClientsController(IInsuranceService insuranceService) {
      this.insuranceService = insuranceService ?? throw new ArgumentNullException(nameof(insuranceService));
    }

    [HttpGet]
    public async Task<IEnumerable<ClientResponse>> GetAllClients() {
      var clients = await insuranceService.GetAllClientsAsync();
      var response = clients.Select(client => new ClientResponse(client)).ToArray();

      return response;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientCreationRequest creationRequest) {
      if (string.IsNullOrEmpty(creationRequest.Name)) {
        return BadRequest();
      }

      var client = await insuranceService.CreateClientAsync(creationRequest.Name);
      var response = new ClientResponse(client);
      return Ok(response);
    }

    [HttpDelete]
    [Route("{clientIdString}")]
    public async Task<IActionResult> DeleteClient([FromRoute] string clientIdString) {
      Guid clientId;
      try {
        clientId = Guid.Parse(clientIdString);
      } catch (FormatException) {
        return BadRequest();
      }

      await insuranceService.DeleteClientAsync(clientId);
      return Ok();
    }

    [HttpPut]
    [Route("{clientIdString}/policies")]
    public async Task<IActionResult> UpdateClientPolicies([FromRoute] string clientIdString, [FromBody] string[] policyIdStrings) {
      if (string.IsNullOrEmpty(clientIdString) || policyIdStrings.Any(string.IsNullOrEmpty)) {
        return BadRequest();
      }

      Guid clientId;
      Guid[] desiredPolicyIds;

      try {
        clientId = Guid.Parse(clientIdString);
        desiredPolicyIds = policyIdStrings.Select(Guid.Parse).ToArray();
      } catch (FormatException) {
        return BadRequest();
      }

      Client client;
      try {
        client = await insuranceService.GetClientAsync(clientId);
      } catch (ResourceNotFoundException) {
        return NotFound();
      }

      var actualPolicyIds = client.AssignedPolicies.Select(policy => policy.Id);
      var idsToAdd = desiredPolicyIds.Except(actualPolicyIds);
      var idsToRemove = actualPolicyIds.Except(desiredPolicyIds);

      foreach (var idToAdd in idsToAdd) {
        await insuranceService.AssignPolicyToClientAsync(idToAdd, clientId);
      }

      foreach (var idToRemove in idsToRemove) {
        await insuranceService.RemovePolicyFromClientAsync(idToRemove, clientId);
      }

      client = await insuranceService.GetClientAsync(clientId);
      var response = new ClientResponse(client);
      return Ok(response);
    }
  }
}
