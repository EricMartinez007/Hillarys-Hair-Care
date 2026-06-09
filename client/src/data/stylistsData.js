export async function getStylists() {
  const response = await fetch('/api/stylists')
  return response.json()
}

export async function getAllStylists() {
  const response = await fetch('/api/stylists/all')
  return response.json()
}

export async function createStylist(stylistData) {
  return fetch('/api/stylists', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(stylistData)
  })
}

export async function deactivateStylist(id) {
  return fetch(`/api/stylists/${id}/deactivate`, { method: 'PATCH' })
}

export async function activateStylist(id) {
  return fetch(`/api/stylists/${id}/activate`, { method: 'PATCH' })
}
