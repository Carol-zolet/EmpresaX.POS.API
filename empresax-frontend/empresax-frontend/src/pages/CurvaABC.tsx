export default function CurvaABC() {
  return (
    <div>
      <h1 className="page-title">An?lise Curva ABC</h1>
      <div className="grid grid-3">
        <div className="metric-card green">
          <p className="metric-title">Categoria A</p>
          <p className="metric-value">12 itens</p>
          <p className="metric-subtitle green">R$ 890.500</p>
        </div>
        <div className="metric-card yellow">
          <p className="metric-title">Categoria B</p>
          <p className="metric-value">25 itens</p>
          <p className="metric-subtitle yellow">R$ 245.800</p>
        </div>
        <div className="metric-card red">
          <p className="metric-title">Categoria C</p>
          <p className="metric-value">48 itens</p>
          <p className="metric-subtitle red">R$ 87.200</p>
        </div>
      </div>
      <div className="card">
        <h3 className="section-title">An?lise de Import?ncia</h3>
        <p>Sistema de curva ABC em desenvolvimento. Permitir? an?lise de clientes e produtos por import?ncia estrat?gica.</p>
      </div>
    </div>
  );
}
