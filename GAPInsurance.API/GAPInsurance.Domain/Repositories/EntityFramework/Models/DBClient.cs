﻿using GAPInsurance.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Models {
  [Table("Clients")]
  public class DBClient {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DBClientCoverage> ClientCoverages { get; set; }

    public DBClient() { }

    public DBClient(Client client) {
      if (client == null) {
        throw new ArgumentNullException(nameof(client));
      }

      Id = client.Id;
      Name = client.Name;
      // Assuming this constructor is only used for _new_ clients without assigned policies
      ClientCoverages = new List<DBClientCoverage>();
    }

    public Client ToModel(bool includePolicies) {
      IEnumerable<InsurancePolicy> appliedPolicies;
      if (includePolicies) {
        appliedPolicies = ClientCoverages
          .Select(cCoverage => cCoverage.Policy.ToModel(false))
          .ToArray()
          .AsEnumerable();
      } else {
        appliedPolicies = new InsurancePolicy[0];
      }
      
      return new Client(Id, Name, appliedPolicies);
    }
  }
}
