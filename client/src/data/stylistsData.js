export async function getStylists() {
  const response = await fetch('/api/stylists')
  return response.json()
}
