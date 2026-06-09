import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button, Table } from 'reactstrap'
import { getAllStylists, deactivateStylist, activateStylist } from '../../data/stylistsData'

export default function StylistsList() {
  const [stylists, setStylists] = useState([])
  const navigate = useNavigate()

  useEffect(() => {
    getAllStylists().then(setStylists)
  }, [])

  const handleDeactivate = async (id) => {
    const res = await deactivateStylist(id)
    if (res.ok) {
      setStylists(prev =>
        prev.map(s => s.id === id ? { ...s, isActive: false } : s)
      )
    }
  }

  const handleActivate = async (id) => {
    const res = await activateStylist(id)
    if (res.ok) {
      setStylists(prev =>
        prev.map(s => s.id === id ? { ...s, isActive: true } : s)
      )
    }
  }

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Stylists</h2>
        <Button color="primary" onClick={() => navigate('/stylists/new')}>
          Add stylist
        </Button>
      </div>

      <Table striped bordered>
        <thead>
          <tr>
            <th>Name</th>
            <th>Phone</th>
            <th>Email</th>
            <th>Status</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {stylists.map(s => (
            <tr key={s.id} style={s.isActive ? {} : { opacity: 0.4 }}>
              <td>{s.name}</td>
              <td>{s.phone}</td>
              <td>{s.email}</td>
              <td>{s.isActive ? 'Active' : 'Inactive'}</td>
              <td>
                {s.isActive
                  ? <Button size="sm" outline color="danger" onClick={() => handleDeactivate(s.id)}>Deactivate</Button>
                  : <Button size="sm" outline color="success" onClick={() => handleActivate(s.id)}>Activate</Button>
                }
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  )
}
