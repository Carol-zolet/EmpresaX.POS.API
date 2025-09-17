import { PieChart, Pie, Cell, ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip } from "recharts";
import { useState, useEffect } from "react";

interface Estatisticas {
  sucesso: boolean;
  total: number;
  totalFormatado: string;
  pendentes: number;
  pendentesFormatado: string;
  pagas: number;
  pagasFormatado: string;
  vencidas: number;
  vencidasFormatado: string;
  quantidadeTotal: number;
  quantidadePendentes: number;
  quantidadeVencidas: number;
}

export default function Dashboard() {
  const [stats, setStats] = useState<Estatisticas | null>(null);
  const [loading, setLoading] = useState(true);
  const [apiError, setApiError] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('http://localhost:5245/api/v1/financeiro/contas-pagar/estatisticas');
        if (response.ok) {
          const data = await response.json();
          setStats(data);
          setApiError(false);
        } else {
          setApiError(true);
        }
      } catch (error) {
        console.error('Erro ao carregar dados:', error);
        setApiError(true);
        // Dados mockados para demonstração
        setStats({
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
        });
      }
      setLoading(false);
    };

    fetchData();
    const interval = setInterval(fetchData, 30000); // Atualizar a cada 30s
    return () => clearInterval(interval);
  }, []);

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center min-h-96">
        <div className="text-xl text-gray-600">Carregando dashboard...</div>
      </div>
    );
  }

  const pieData = [
    { name: "Pagas", value: stats?.pagas || 0, color: "#10b981" },
    { name: "Pendentes", value: stats?.pendentes || 0, color: "#f59e0b" },
    { name: "Vencidas", value: stats?.vencidas || 0, color: "#ef4444" },
  ].filter(item => item.value > 0);

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard Financeiro</h1>
        <div className="flex items-center space-x-2">
          {apiError && (
            <span className="bg-red-100 text-red-700 px-3 py-1 rounded-full text-sm">
              API Offline - Dados de Exemplo
            </span>
          )}
          <div className="text-sm text-gray-500">
            Última atualização: {new Date().toLocaleString('pt-BR')}
          </div>
        </div>
      </div>

      {/* Cards de Resumo */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-blue-500">
          <p className="text-sm font-medium text-gray-600">Total de Contas</p>
          <p className="text-2xl font-bold text-gray-900">{stats?.quantidadeTotal || 0}</p>
          <p className="text-lg text-blue-600">{stats?.totalFormatado}</p>
        </div>

        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-500">
          <p className="text-sm font-medium text-gray-600">Contas Pagas</p>
          <p className="text-2xl font-bold text-gray-900">
            {(stats?.quantidadeTotal || 0) - (stats?.quantidadePendentes || 0) - (stats?.quantidadeVencidas || 0)}
          </p>
          <p className="text-lg text-green-600">{stats?.pagasFormatado}</p>
        </div>

        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-yellow-500">
          <p className="text-sm font-medium text-gray-600">Contas Pendentes</p>
          <p className="text-2xl font-bold text-gray-900">{stats?.quantidadePendentes || 0}</p>
          <p className="text-lg text-yellow-600">{stats?.pendentesFormatado}</p>
        </div>

        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-red-500">
          <p className="text-sm font-medium text-gray-600">Contas Vencidas</p>
          <p className="text-2xl font-bold text-gray-900">{stats?.quantidadeVencidas || 0}</p>
          <p className="text-lg text-red-600">{stats?.vencidasFormatado}</p>
        </div>
      </div>

      {/* Gráfico de Pizza */}
      <div className="bg-white shadow-lg rounded-xl p-6">
        <h3 className="text-lg font-semibold mb-4 text-gray-900">Distribuição por Status</h3>
        <div className="h-80">
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <Pie 
                data={pieData} 
                dataKey="value" 
                nameKey="name"
                cx="50%" 
                cy="50%" 
                outerRadius={100} 
                label={({name, percent}) => `${name}: ${(percent * 100).toFixed(1)}%`}
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
