export interface Policy {
  name: string,
  description: string,
  premiumPrice: number,
  coverageStartDate: string,
  coverageLength: number,
  riskLevelId: number,
  fireCoverage: number,
  theftCoverage: number,
  lossCoverage: number,
  coveredClients: string[]
}
