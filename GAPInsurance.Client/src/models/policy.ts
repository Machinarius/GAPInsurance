export interface Policy {
  id: string,
  name: string,
  description: string,
  premiumPrice: number,
  coverageStartDate: string,
  coverageLength: number,
  riskLevel: string,
  earthquakeCoverage: number,
  fireCoverage: number,
  theftCoverage: number,
  lossCoverage: number,
  coveredClients: string[]
}
