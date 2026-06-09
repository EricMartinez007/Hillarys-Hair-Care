import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Form, FormGroup, Label, Input, Button } from 'reactstrap'
import { createCustomer } from '../../data/customersData'

export default function NewCustomerForm() {
  const [formData, setFormData] = useState({ name: '', phone: '', email: '' })
  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    e.preventDefault()
    const response = await createCustomer(formData)
    if (response.ok) {
      navigate('/customers')
    }
  }

  return (
    <div className="container mt-4">
      <h2>Add customer</h2>

      <div className="card p-4">
        <Form onSubmit={handleSubmit}>

          <FormGroup>
            <Label for="name">Name</Label>
            <Input
              type="text"
              id="name"
              value={formData.name}
              onChange={e => setFormData({ ...formData, name: e.target.value })}
              required
            />
          </FormGroup>

          <FormGroup>
            <Label for="phone">Phone</Label>
            <Input
              type="tel"
              id="phone"
              value={formData.phone}
              onChange={e => setFormData({ ...formData, phone: e.target.value })}
              required
            />
          </FormGroup>

          <FormGroup>
            <Label for="email">Email</Label>
            <Input
              type="email"
              id="email"
              value={formData.email}
              onChange={e => setFormData({ ...formData, email: e.target.value })}
              required
            />
          </FormGroup>

          <div className="d-flex justify-content-end gap-2">
            <Button color="secondary" outline onClick={() => navigate('/customers')}>
              Cancel
            </Button>
            <Button color="primary" type="submit">
              Add customer
            </Button>
          </div>

        </Form>
      </div>
    </div>
  )
}
