import { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { Form, FormGroup, Label, Input, Button } from 'reactstrap'
import { getStylists } from '../../data/stylistsData'
import { getCustomers } from '../../data/customersData'
import { getServices } from '../../data/servicesData'
import { getAppointment, updateAppointment } from '../../data/appointmentsData'

export default function EditAppointmentForm() {
  const { id } = useParams()
  const [stylists, setStylists] = useState([])
  const [customers, setCustomers] = useState([])
  const [services, setServices] = useState([])
  const [formData, setFormData] = useState({
    customerId: '',
    stylistId: '',
    scheduledAt: '',
    serviceIds: []
  })
  const navigate = useNavigate()

  useEffect(() => {
    getStylists().then(setStylists)
    getCustomers().then(setCustomers)
    getServices().then(setServices)
    getAppointment(id).then(appt => {
      setFormData({
        customerId: appt.customerId,
        stylistId: appt.stylistId,
        scheduledAt: appt.scheduledAt.slice(0, 16),
        serviceIds: appt.services.map(s => s.id)
      })
    })
  }, [])

  const handleServiceToggle = (serviceId) => {
    setFormData(prev => ({
      ...prev,
      serviceIds: prev.serviceIds.includes(serviceId)
        ? prev.serviceIds.filter(s => s !== serviceId)
        : [...prev.serviceIds, serviceId]
    }))
  }

  const total = services
    .filter(s => formData.serviceIds.includes(s.id))
    .reduce((sum, s) => sum + s.price, 0)

  const handleSubmit = async (e) => {
    e.preventDefault()
    const response = await updateAppointment(id, {
      stylistId: parseInt(formData.stylistId),
      customerId: parseInt(formData.customerId),
      scheduledAt: formData.scheduledAt,
      serviceIds: formData.serviceIds
    })
    if (response.ok) {
      navigate('/appointments')
    }
  }

  return (
    <div className="container mt-4">
      <h2>Edit appointment</h2>

      <div className="card p-4">
        <Form onSubmit={handleSubmit}>

          <FormGroup>
            <Label for="customerId">Customer</Label>
            <Input
              type="select"
              id="customerId"
              value={formData.customerId}
              onChange={e => setFormData({ ...formData, customerId: e.target.value })}
              required
            >
              <option value="">Select a customer</option>
              {customers.map(c => (
                <option key={c.id} value={c.id}>{c.name}</option>
              ))}
            </Input>
          </FormGroup>

          <FormGroup>
            <Label for="stylistId">Stylist <span className="text-muted">(active only)</span></Label>
            <Input
              type="select"
              id="stylistId"
              value={formData.stylistId}
              onChange={e => setFormData({ ...formData, stylistId: e.target.value })}
              required
            >
              <option value="">Select a stylist</option>
              {stylists.map(s => (
                <option key={s.id} value={s.id}>{s.name}</option>
              ))}
            </Input>
          </FormGroup>

          <FormGroup>
            <Label for="scheduledAt">Date &amp; Time <span className="text-muted">(on the hour)</span></Label>
            <Input
              type="datetime-local"
              id="scheduledAt"
              step="3600"
              value={formData.scheduledAt}
              onChange={e => setFormData({ ...formData, scheduledAt: e.target.value })}
              required
            />
          </FormGroup>

          <FormGroup>
            <Label>Services <span className="text-muted">(pick one or more)</span></Label>
            {services.map(s => (
              <div
                key={s.id}
                className={`d-flex align-items-center justify-content-between p-3 mb-2 border rounded ${formData.serviceIds.includes(s.id) ? 'border-primary bg-primary bg-opacity-10' : ''}`}
              >
                <FormGroup check className="mb-0">
                  <Input
                    type="checkbox"
                    id={`service-${s.id}`}
                    checked={formData.serviceIds.includes(s.id)}
                    onChange={() => handleServiceToggle(s.id)}
                  />
                  <Label check htmlFor={`service-${s.id}`}>{s.name}</Label>
                </FormGroup>
                <span>${s.price.toFixed(2)}</span>
              </div>
            ))}
          </FormGroup>

          <div className="d-flex justify-content-between align-items-center p-3 rounded mb-4 bg-warning bg-opacity-50">
            <span>Appointment total</span>
            <strong>${total.toFixed(2)}</strong>
          </div>

          <div className="d-flex justify-content-end gap-2">
            <Button color="secondary" outline onClick={() => navigate('/appointments')}>
              Cancel
            </Button>
            <Button color="primary" type="submit">
              Save changes
            </Button>
          </div>

        </Form>
      </div>
    </div>
  )
}
