export async function getCustomers() {
  const response = await fetch('/api/customers')
  return response.json()
}

export async function createCustomer(customerData) {
  return fetch('/api/customers', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(customerData)
  })
}
