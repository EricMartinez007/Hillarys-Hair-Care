export async function getCustomers() {
  const response = await fetch('/api/customers')
  return response.json()
}
