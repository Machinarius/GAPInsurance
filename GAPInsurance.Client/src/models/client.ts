export interface Client {
  id: string,
  name: string,
  assignedPolicies: [PolicyValue]
}

export interface PolicyValue {
  id: string,
  name: string
}