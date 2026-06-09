import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Form, FormGroup, Label, Input, Button } from 'reactstrap'
import { getStylists } from '../../data/stylistsData'
import { getCustomers } from '../../data/customersData'
import { getServices } from '../../data/servicesData'
import { createAppointment } from '../../data/appointmentsData'

export default function NewAppointmentForm() {
  const [stylists, setStylists] = useState([])
  const [customers, setCustomers] = useState([])
  const [services, setServices] = useState([])
  const [formData, setFormData] = useState({
    customerId: '',
    stylistId: '',
    scheduledAt: '',
    serviceIds: []
  })
  const [submitted, setSubmitted] = useState(false)
  const navigate = useNavigate()

  useEffect(() => {
    getStylists().then(setStylists)
    getCustomers().then(setCustomers)
    getServices().then(setServices)
  }, [])

  // This is the toggle logic for selecting the different services. the function uses the previous state to update and keeps the other fields intact by using the spread operator.
  // The way this logic works is by checking if the service ID is already in the serviceIDs array in formData. If its already there (the checkbox was checked and now they are unchecking it) .filter() will remove it. If its not there (the checkbox was unchecked and now the user is checking it) it will spread the existing array and adds the new id.
  const handleServiceToggle = (id) => {
    setFormData(prev => ({
      ...prev,
      serviceIds: prev.serviceIds.includes(id)
        ? prev.serviceIds.filter(s => s !== id)
        : [...prev.serviceIds, id]
    }))
  }

  const total = services
    .filter(s => formData.serviceIds.includes(s.id))
    .reduce((sum, s) => sum + s.price, 0)

  const handleCancel = () => {
    navigate('/appointments')
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    const response = await createAppointment({
      stylistId: parseInt(formData.stylistId),
      customerId: parseInt(formData.customerId),
      scheduledAt: formData.scheduledAt,
      serviceIds: formData.serviceIds
    })
    if (response.ok) {
      setSubmitted(true)
    }
  }

  if (submitted) {
    return (
      <div className="container mt-4">
        <h2>Appointment booked!</h2>
        <div className="d-flex gap-2 mt-3">
          <Button
            color="primary"
            onClick={() => {
              setSubmitted(false)
              setFormData({ customerId: '', stylistId: '', scheduledAt: '', serviceIds: [] })
            }}
          >
            Book another
          </Button>
          <Button color="outline-secondary" onClick={() => navigate('/appointments')}>
            View appointments
          </Button>
        </div>
      </div>
    )
  }

  return (
    <div className="container mt-4">
      <h2>Schedule appointment</h2>
      <p className="text-muted">Same form is reused to edit an existing one</p>

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
            <Button color="secondary" outline onClick={handleCancel}>
              Cancel
            </Button>
            <Button color="primary" type="submit">
              Save appointment
            </Button>
          </div>

        </Form>
      </div>
    </div>
  )
}
