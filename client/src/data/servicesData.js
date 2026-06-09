export async function getServices() {
  const response = await fetch('/api/services')
  return response.json()
}
