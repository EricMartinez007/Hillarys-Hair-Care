import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button, Input } from 'reactstrap'
import { getAppointments, cancelAppointment } from '../../data/appointmentsData'

export default function AppointmentsList() {
  const [appointments, setAppointments] = useState([])
  const [filter, setFilter] = useState('All')
  const navigate = useNavigate()

  useEffect(() => {
    getAppointments().then(setAppointments)
  }, [])

  const handleCancel = async (id) => {
    const res = await cancelAppointment(id)
    if (res.ok) {
      setAppointments(prev =>
        prev.map(a => a.id === id ? { ...a, status: 'Cancelled' } : a)
      )
    }
  }

  const filtered = filter === 'All'
    ? appointments
    : appointments.filter(a => a.status === filter)

  return (
    <div className="appts-page">
      <div className="appts-header">
        <div>
          <h2 className="appts-title">Appointments</h2>
          <p className="appts-subtitle">Everything booked, newest first</p>
        </div>
        <Button color="primary" onClick={() => navigate('/appointments/new')}>
          + New appointment
        </Button>
      </div>

      <div className="filter-dropdown">
        <Input type="select" value={filter} onChange={e => setFilter(e.target.value)} style={{ width: 180 }}>
          <option>All</option>
          <option>Scheduled</option>
          <option>Cancelled</option>
          <option>Completed</option>
        </Input>
      </div>

      <div className="appt-list">
        {filtered.length === 0 && <p className="text-muted">No appointments found.</p>}
        {filtered.map(appt => {
          const isCancelled = appt.status === 'Cancelled'
          const date = new Date(appt.scheduledAt)
          const timeStr = date.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit' })
          const dateStr = date.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' })

          return (
            <div key={appt.id} className={`appt-card ${isCancelled ? 'appt-card--cancelled' : ''}`}>
              <div className="appt-time-col">
                <span className="appt-time">{timeStr}</span>
                <span className="appt-date">{dateStr}</span>
              </div>

              <div className="appt-details">
                <div className="appt-who">
                  <span className="appt-customer">{appt.customerName}</span>
                  <span className="appt-stylist"> · with {appt.stylistName}</span>
                </div>
                <div className="appt-tags">
                  {appt.services.map(s => (
                    <span key={s.id} className="service-tag">{s.name}</span>
                  ))}
                  <span className={`status-badge status-badge--${appt.status.toLowerCase()}`}>
                    {appt.status}
                  </span>
                </div>
              </div>

              <div className="appt-actions">
                <span className={`appt-cost ${isCancelled ? 'appt-cost--cancelled' : ''}`}>
                  ${appt.totalCost}
                </span>
                {appt.status === 'Scheduled' && (
                  <div className="appt-buttons">
                    <Button size="sm" outline>Edit</Button>
                    <Button size="sm" outline color="danger" onClick={() => handleCancel(appt.id)}>
                      Cancel
                    </Button>
                  </div>
                )}
              </div>
            </div>
          )
        })}
      </div>
    </div>
  )
}
