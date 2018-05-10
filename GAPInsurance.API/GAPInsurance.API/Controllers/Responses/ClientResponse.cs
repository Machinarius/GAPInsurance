using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GAPInsurance.API.Controllers.Responses {
  public class ClientResponse {
    public string Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<string> AssignedPolicies { get; set; }

    public ClientResponse() { }

    public ClientResponse(Client model) {
      if (model == null) {
        throw new ArgumentNullException(nameof(model));
      }

      Id = model.Id.ToString();
      Name = model.Name;
      AssignedPolicies = model.AssignedPolicies.Select(policy => policy.Name).ToArray();
    }
  }
}
