export default function Caixa() {
  return (
    <div>
      <h1 className="page-title">DRE / Fluxo de Caixa</h1>
      <div className="grid grid-4">
        <div className="metric-card blue">
          <p className="metric-title">Receita Bruta</p>
          <p className="metric-value">R$ 150.000</p>
        </div>
        <div className="metric-card green">
          <p className="metric-title">Lucro Bruto</p>
          <p className="metric-value">R$ 87.500</p>
        </div>
        <div className="metric-card purple">
          <p className="metric-title">Lucro L?quido</p>
          <p className="metric-value">R$ 55.500</p>
        </div>
        <div className="metric-card yellow">
          <p className="metric-title">Margem L?quida</p>
          <p className="metric-value">37.0%</p>
        </div>
      </div>
      <div className="card">
        <h3 className="section-title">Demonstrativo de Resultados</h3>
        <p>Sistema de DRE e fluxo de caixa em desenvolvimento. Incluir? gr?ficos de evolu??o mensal e indicadores financeiros.</p>
      </div>
    </div>
  );
}
