import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button, Table } from 'reactstrap'
import { getCustomers } from '../../data/customersData'

export default function CustomersList() {
  const [customers, setCustomers] = useState([])
  const navigate = useNavigate()

  useEffect(() => {
    getCustomers().then(setCustomers)
  }, [])

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Customers</h2>
        <Button color="primary" onClick={() => navigate('/customers/new')}>
          Add customer
        </Button>
      </div>

      <Table striped bordered>
        <thead>
          <tr>
            <th>Name</th>
            <th>Phone</th>
            <th>Email</th>
          </tr>
        </thead>
        <tbody>
          {customers.map(c => (
            <tr key={c.id}>
              <td>{c.name}</td>
              <td>{c.phone}</td>
              <td>{c.email}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  )
}
