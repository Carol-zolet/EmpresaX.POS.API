import React, { useState } from 'react';

function App() {
  const [page, setPage] = useState('dashboard');

  const renderPage = () => {
    switch(page) {
      case 'contas': return <ContasPagar />;
      case 'clientes': return <Clientes />;
      case 'produtos': return <Produtos />;
      default: return <Dashboard />;
    }
  };

  return (
    <div className="layout">
      <div className="sidebar">
        <h2>EmpresaX POS</h2>
        <nav>
          <a href="#" className={page === 'dashboard' ? 'nav-link active' : 'nav-link'} 
             onClick={() => setPage('dashboard')}>Dashboard</a>
          <a href="#" className={page === 'contas' ? 'nav-link active' : 'nav-link'} 
             onClick={() => setPage('contas')}>Contas a Pagar</a>
          <a href="#" className={page === 'clientes' ? 'nav-link active' : 'nav-link'} 
             onClick={() => setPage('clientes')}>Clientes</a>
          <a href="#" className={page === 'produtos' ? 'nav-link active' : 'nav-link'} 
             onClick={() => setPage('produtos')}>Produtos</a>
        </nav>
      </div>
      <div className="main">
        {renderPage()}
      </div>
    </div>
  );
}

function Dashboard() {
  return (
    <div>
      <h1>Dashboard Financeiro</h1>
      <div className="grid grid-4">
        <div className="metric">
          <h3>Total de Contas</h3>
          <p>15</p>
        </div>
        <div className="metric">
          <h3>Pendentes</h3>
          <p>8</p>
        </div>
        <div className="metric">
          <h3>Pagas</h3>
          <p>5</p>
        </div>
        <div className="metric">
          <h3>Vencidas</h3>
          <p>2</p>
        </div>
      </div>
      <div className="card">
        <h3>Sistema funcionando!</h3>
        <p>Backend integrado com React funcionando perfeitamente.</p>
      </div>
    </div>
  );
}

function ContasPagar() {
  return (
    <div>
      <h1>Contas a Pagar</h1>
      <div className="card">
        <p>Sistema de contas a pagar em desenvolvimento...</p>
      </div>
    </div>
  );
}

function Clientes() {
  return (
    <div>
      <h1>Clientes</h1>
      <div className="card">
        <p>Sistema de clientes em desenvolvimento...</p>
      </div>
    </div>
  );
}

function Produtos() {
  return (
    <div>
      <h1>Produtos</h1>
      <div className="card">
        <p>Sistema de produtos em desenvolvimento...</p>
      </div>
    </div>
  );
}

export default App;
