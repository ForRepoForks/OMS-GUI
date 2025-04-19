import './App.css';
import { NavLink, Outlet } from 'react-router-dom';

function App() {
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
            to="/customers"
            className={({ isActive }) => (isActive ? 'sidebar-link active' : 'sidebar-link')}
          >
            Customers
          </NavLink>
          <NavLink
            to="/products"
            className={({ isActive }) => (isActive ? 'sidebar-link active' : 'sidebar-link')}
          >
            Products
          </NavLink>
        </nav>
      </aside>
      <main className="main-content">
        <header className="topbar">
          <span className="topbar-title">Order Management System</span>
        </header>
        <section className="content-area">
          <Outlet />
        </section>
      </main>
    </div>
  );
}

export default App;
