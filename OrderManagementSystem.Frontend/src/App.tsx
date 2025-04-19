import './App.css'

function App() {
  return (
    <div className="app-layout">
      <aside className="sidebar">
        <div className="sidebar-header">OMS</div>
        <nav className="sidebar-nav">
          <a href="#" className="sidebar-link">Dashboard</a>
          <a href="#" className="sidebar-link">Orders</a>
          <a href="#" className="sidebar-link">Products</a>
          <a href="#" className="sidebar-link">Customers</a>
        </nav>
      </aside>
      <main className="main-content">
        <header className="topbar">
          <span className="topbar-title">Order Management System</span>
        </header>
        <section className="content-area">
          <h2>Welcome!</h2>
          <p>This is your main content area. Select a section from the sidebar.</p>
        </section>
      </main>
    </div>
  );
}


export default App
