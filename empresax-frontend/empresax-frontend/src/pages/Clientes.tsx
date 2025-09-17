export default function Clientes() {
  return (
    <div>
      <h1 className="page-title">Gest?o de Clientes</h1>
      <div className="grid grid-3">
        <div className="metric-card blue">
          <p className="metric-title">Total de Clientes</p>
          <p className="metric-value">127</p>
        </div>
        <div className="metric-card green">
          <p className="metric-title">Clientes Ativos</p>
          <p className="metric-value">98</p>
        </div>
        <div className="metric-card yellow">
          <p className="metric-title">Faturamento Total</p>
          <p className="metric-value">R$ 245.680</p>
        </div>
      </div>
      <div className="card">
        <h3 className="section-title">Funcionalidades em Desenvolvimento</h3>
        <p>Esta p?gina permitir? gerenciar clientes, acompanhar hist?rico de compras e m?tricas de faturamento.</p>
      </div>
    </div>
  );
}
