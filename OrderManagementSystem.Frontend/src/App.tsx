import './App.css';
import { NavLink, Outlet } from 'react-router-dom';
import Button from '@mui/material/Button';
import React from 'react';
import api from './api';

function App() {
  const [alert, setAlert] = React.useState<string | null>(null);
  const [alertType, setAlertType] = React.useState<'success' | 'error'>('success');
  const handleSeed = async () => {
    try {
      await api.post('/api/seed');
      setAlert('Database seeded with test data.');
      setAlertType('success');
    } catch (err) {
      setAlert('Failed to seed database.');
      setAlertType('error');
    }
    setTimeout(() => setAlert(null), 4000);
  };

  return (
    <div className="app-layout">
      <aside className="sidebar">
        <div className="sidebar-header">OMS</div>
        <nav className="sidebar-nav">
          <NavLink
            to="/orders"
            className={({ isActive }) => (isActive ? 'sidebar-link active' : 'sidebar-link')}
          >
            Orders
          </NavLink>
          <NavLink
            to="/products"
            className={({ isActive }) => (isActive ? 'sidebar-link active' : 'sidebar-link')}
          >
            Products
          </NavLink>
          <Button
            variant="contained"
            color="secondary"
            style={{ marginTop: 16 }}
            onClick={handleSeed}
            data-testid="seed-db-btn"
          >
            Seed Database
          </Button>
        </nav>
      </aside>
      <main className="main-content">
        <header className="topbar">
          <span className="topbar-title">Order Management System</span>
        </header>
        {alert && (
          <div style={{
            background: alertType === 'success' ? '#d4edda' : '#f8d7da',
            color: alertType === 'success' ? '#155724' : '#721c24',
            padding: '12px 24px',
            margin: '16px',
            borderRadius: 6,
            border: `1px solid ${alertType === 'success' ? '#c3e6cb' : '#f5c6cb'}`,
            zIndex: 9999,
            position: 'relative',
            textAlign: 'center',
          }}>
            {alert}
          </div>
        )}
        <section className="content-area">
          <Outlet />
        </section>
      </main>
    </div>
  );
}

export default App;
