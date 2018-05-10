export class PolicyCreationRequest {
  constructor(
    public name: string,
    public description: string,
    public premiumPrice: number,
    public coverageStartDate: string,
    public coverageLength: number,
    public riskLevelId: number,
    public earthquakeCoverage: number,
    public fireCoverage: number,
    public theftCoverage: number,
    public lossCoverage: number
  ) { }
}
