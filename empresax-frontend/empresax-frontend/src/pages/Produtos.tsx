export default function Produtos() {
  return (
    <div>
      <h1 className="page-title">Cat?logo de Produtos</h1>
      <div className="grid grid-4">
        <div className="metric-card blue">
          <p className="metric-title">Total Produtos</p>
          <p className="metric-value">89</p>
        </div>
        <div className="metric-card green">
          <p className="metric-title">Faturamento</p>
          <p className="metric-value">R$ 1.247.800</p>
        </div>
        <div className="metric-card yellow">
          <p className="metric-title">Custos</p>
          <p className="metric-value">R$ 758.200</p>
        </div>
        <div className="metric-card purple">
          <p className="metric-title">Margem Geral</p>
          <p className="metric-value">39.2%</p>
        </div>
      </div>
      <div className="card">
        <h3 className="section-title">Funcionalidades em Desenvolvimento</h3>
        <p>Esta p?gina permitir? gerenciar produtos, controlar estoque e calcular margens de lucro.</p>
      </div>
    </div>
  );
}
