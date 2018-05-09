using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GAPInsurance.Domain.Repositories.EntityFramework.Migrations {
  public partial class InitialDatabase : Migration {
    protected override void Up(MigrationBuilder migrationBuilder) {
      migrationBuilder.CreateTable(
          name: "Clients",
          columns: table => new {
            Id = table.Column<Guid>(nullable: false),
            Name = table.Column<string>(nullable: true)
          },
          constraints: table => {
            table.PrimaryKey("PK_Clients", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "Policies",
          columns: table => new {
            Id = table.Column<Guid>(nullable: false),
            CoverageLengthInMonths = table.Column<int>(nullable: false),
            CoverageStartDate = table.Column<DateTime>(nullable: false),
            Description = table.Column<string>(nullable: true),
            InsuredRiskLevel = table.Column<int>(nullable: false),
            Name = table.Column<string>(nullable: true),
            PremiumCostInDollars = table.Column<float>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_Policies", x => x.Id);
          });

      migrationBuilder.CreateTable(
          name: "ClientPolicies",
          columns: table => new {
            PolicyId = table.Column<Guid>(nullable: false),
            ClientId = table.Column<Guid>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_ClientPolicies", x => new { x.PolicyId, x.ClientId });
            table.ForeignKey(
                      name: "FK_ClientPolicies_Clients_ClientId",
                      column: x => x.ClientId,
                      principalTable: "Clients",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_ClientPolicies_Policies_PolicyId",
                      column: x => x.PolicyId,
                      principalTable: "Policies",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "PolicyCoverages",
          columns: table => new {
            PolicyId = table.Column<Guid>(nullable: false),
            Coverage = table.Column<int>(nullable: false),
            Percentage = table.Column<float>(nullable: false)
          },
          constraints: table => {
            table.PrimaryKey("PK_PolicyCoverages", x => new { x.PolicyId, x.Coverage });
            table.UniqueConstraint("AK_PolicyCoverages_Coverage_PolicyId", x => new { x.Coverage, x.PolicyId });
            table.ForeignKey(
                      name: "FK_PolicyCoverages_Policies_PolicyId",
                      column: x => x.PolicyId,
                      principalTable: "Policies",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_ClientPolicies_ClientId",
          table: "ClientPolicies",
          column: "ClientId");
    }

    protected override void Down(MigrationBuilder migrationBuilder) {
      migrationBuilder.DropTable(
          name: "ClientPolicies");

      migrationBuilder.DropTable(
          name: "PolicyCoverages");

      migrationBuilder.DropTable(
          name: "Clients");

      migrationBuilder.DropTable(
          name: "Policies");
    }
  }
}
