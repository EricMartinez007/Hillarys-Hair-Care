export async function getAppointments() {
  const response = await fetch('/api/appointments')
  return response.json()
}

export async function createAppointment(appointmentData) {
  const response = await fetch('/api/appointments', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(appointmentData)
  })
  return response
}

export async function cancelAppointment(id) {
  const response = await fetch(`/api/appointments/${id}/cancel`, {
    method: 'PATCH'
  })
  return response
}
