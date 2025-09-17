import { PieChart, Pie, Cell, ResponsiveContainer, Tooltip } from "recharts";
import { useApiData } from "../hooks/useApiData";
import { Estatisticas } from "../types";
import Loading from "../components/Loading";

export default function Dashboard() {
  const fallbackStats: Estatisticas = {
    sucesso: true,
    total: 12450.80,
    totalFormatado: "R$ 12.450,80",
    pendentes: 8500.50,
    pendentesFormatado: "R$ 8.500,50",
    pagas: 3200.30,
    pagasFormatado: "R$ 3.200,30",
    vencidas: 750.00,
    vencidasFormatado: "R$ 750,00",
    quantidadeTotal: 15,
    quantidadePendentes: 8,
    quantidadeVencidas: 2
  };

  const { data: stats, loading, isOffline } = useApiData<Estatisticas>({
    url: 'http://localhost:5245/api/v1/financeiro/contas-pagar/estatisticas',
    fallbackData: fallbackStats,
    refreshInterval: 30000
  });

  if (loading) {
    return <Loading message="Carregando dashboard..." />;
  }

  const pieData = [
    { name: "Pagas", value: stats?.pagas || 0, color: "#38a169" },
    { name: "Pendentes", value: stats?.pendentes || 0, color: "#d69e2e" },
    { name: "Vencidas", value: stats?.vencidas || 0, color: "#e53e3e" },
  ].filter(item => item.value > 0);

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
        <h1 className="page-title">Dashboard Financeiro</h1>
        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          {isOffline && (
            <span className="status-badge status-error">
              API Offline - Dados de Exemplo
            </span>
          )}
          <div style={{ fontSize: '0.875rem', color: '#718096' }}>
            ?ltima atualiza??o: {new Date().toLocaleString('pt-BR')}
          </div>
        </div>
      </div>

      <div className="grid grid-4">
        <div className="metric-card blue">
          <p className="metric-title">Total de Contas</p>
          <p className="metric-value">{stats?.quantidadeTotal || 0}</p>
          <p className="metric-subtitle blue">{stats?.totalFormatado}</p>
        </div>

        <div className="metric-card green">
          <p className="metric-title">Contas Pagas</p>
          <p className="metric-value">
            {(stats?.quantidadeTotal || 0) - (stats?.quantidadePendentes || 0) - (stats?.quantidadeVencidas || 0)}
          </p>
          <p className="metric-subtitle green">{stats?.pagasFormatado}</p>
        </div>

        <div className="metric-card yellow">
          <p className="metric-title">Contas Pendentes</p>
          <p className="metric-value">{stats?.quantidadePendentes || 0}</p>
          <p className="metric-subtitle yellow">{stats?.pendentesFormatado}</p>
        </div>

        <div className="metric-card red">
          <p className="metric-title">Contas Vencidas</p>
          <p className="metric-value">{stats?.quantidadeVencidas || 0}</p>
          <p className="metric-subtitle red">{stats?.vencidasFormatado}</p>
        </div>
      </div>

      <div className="card">
        <h3 className="section-title">Distribui??o por Status</h3>
        <div style={{ height: '320px' }}>
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <Pie 
                data={pieData} 
                dataKey="value" 
                nameKey="name"
                cx="50%" 
                cy="50%" 
                outerRadius={100} 
                label={(entry: any) => {
                  const total = pieData.reduce((sum, item) => sum + item.value, 0);
                  const percent = ((entry.value / total) * 100).toFixed(1);
                  return `${entry.name}: ${percent}%`;
                }}
              >
                {pieData.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip formatter={(value: number) => `R$ ${value.toLocaleString('pt-BR', {minimumFractionDigits: 2})}`} />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
}
