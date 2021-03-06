export interface Policy {
  id: string,
  name: string,
  description: string,
  premiumPrice: number,
  coverageStartDate: string,
  coverageLength: number,
  riskLevelId: number,
  earthquakeCoverage: number,
  fireCoverage: number,
  theftCoverage: number,
  lossCoverage: number,
  coveredClients: ClientValue[]
}

interface ClientValue {
  id: string,
  name: string
}
