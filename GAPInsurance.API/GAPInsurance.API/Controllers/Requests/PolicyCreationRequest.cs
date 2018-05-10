namespace GAPInsurance.API.Controllers.Requests {
  public class PolicyCreationRequest {
    public string Name { get; set; }
    public string Description { get; set; }
    public float PremiumPrice { get; set; }
    public string CoverageStartDate { get; set; }
    public int CoverageLength { get; set; }
    public int RiskLevelId { get; set; }
    public int EarthquakeCoverage { get; set; }
    public int FireCoverage { get; set; }
    public int TheftCoverage { get; set; }
    public int LossCoverage { get; set; }
  }
}
