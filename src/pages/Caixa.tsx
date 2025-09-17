import { useState, useEffect } from "react";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts";

interface DREData {
  receita_bruta: number;
  deducoes: number;
  receita_liquida: number;
  custos: number;
  lucro_bruto: number;
  despesas: number;
  lucro_liquido: number;
  margem_liquida: number;
}

export default function Caixa() {
  const [dreData, setDreData] = useState<DREData | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Dados mockados para DRE
    setTimeout(() => {
      setDreData({
        receita_bruta: 150000,
        deducoes: 7500,
        receita_liquida: 142500,
        custos: 55000,
        lucro_bruto: 87500,
        despesas: 32000,
        lucro_liquido: 55500,
        margem_liquida: 38.9
      });
      setLoading(false);
    }, 1000);
  }, []);

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center">
        <div className="text-lg">Carregando DRE...</div>
      </div>
    );
  }

  const evolucaoMensal = [
    { mes: 'Jan', lucro: 45000, receita: 120000 },
    { mes: 'Fev', lucro: 52000, receita: 135000 },
    { mes: 'Mar', lucro: 48000, receita: 128000 },
    { mes: 'Abr', lucro: 55500, receita: 142500 },
    { mes: 'Mai', lucro: 58000, receita: 148000 },
  ];

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">DRE - Demonstração de Resultados</h1>
        <div className="text-sm text-gray-500">
          Período: {new Date().toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' })}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* DRE Table */}
        <div className="bg-white shadow-lg rounded-xl p-6">
          <h2 className="text-xl font-semibold mb-4 text-gray-900">Demonstrativo do Resultado</h2>
          <table className="w-full">
            <tbody className="divide-y divide-gray-100">
              <tr className="py-3">
                <td className="py-3 text-sm font-semibold text-gray-900">Receita Bruta</td>
                <td className="py-3 text-sm font-bold text-right text-gray-900">
                  R$ {dreData?.receita_bruta.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
              </tr>
              <tr>
                <td className="py-3 text-sm text-gray-600">(-) Deduções</td>
                <td className="py-3 text-sm text-right text-red-600">
                  (R$ {dreData?.deducoes.toLocaleString('pt-BR', {minimumFractionDigits: 2})})
                </td>
              </tr>
              <tr className="border-t-2 border-gray-200">
                <td className="py-3 text-sm font-semibold text-gray-900">Receita Líquida</td>
                <td className="py-3 text-sm font-bold text-right text-blue-600">
                  R$ {dreData?.receita_liquida.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
              </tr>
              <tr>
                <td className="py-3 text-sm text-gray-600">(-) Custos dos Produtos Vendidos</td>
                <td className="py-3 text-sm text-right text-red-600">
                  (R$ {dreData?.custos.toLocaleString('pt-BR', {minimumFractionDigits: 2})})
                </td>
              </tr>
              <tr className="border-t border-gray-200">
                <td className="py-3 text-sm font-semibold text-gray-900">Lucro Bruto</td>
                <td className="py-3 text-sm font-bold text-right text-green-600">
                  R$ {dreData?.lucro_bruto.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
              </tr>
              <tr>
                <td className="py-3 text-sm text-gray-600">(-) Despesas Operacionais</td>
                <td className="py-3 text-sm text-right text-red-600">
                  (R$ {dreData?.despesas.toLocaleString('pt-BR', {minimumFractionDigits: 2})})
                </td>
              </tr>
              <tr className="border-t-2 border-gray-300 bg-gray-50">
                <td className="py-4 text-lg font-bold text-gray-900">Lucro Líquido</td>
                <td className="py-4 text-lg font-bold text-right text-green-700">
                  R$ {dreData?.lucro_liquido.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
              </tr>
              <tr>
                <td className="py-3 text-sm font-medium text-gray-700">Margem Líquida</td>
                <td className="py-3 text-sm font-bold text-right text-blue-700">
                  {dreData?.margem_liquida.toFixed(1)}%
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        {/* Evolução Mensal */}
        <div className="bg-white shadow-lg rounded-xl p-6">
          <h2 className="text-xl font-semibold mb-4 text-gray-900">Evolução Mensal</h2>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart data={evolucaoMensal}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="mes" />
                <YAxis tickFormatter={(value) => `R$ ${(value/1000).toFixed(0)}k`} />
                <Tooltip formatter={(value: number) => `R$ ${value.toLocaleString('pt-BR', {minimumFractionDigits: 2})}`} />
                <Line type="monotone" dataKey="receita" stroke="#3b82f6" strokeWidth={2} name="Receita" />
                <Line type="monotone" dataKey="lucro" stroke="#10b981" strokeWidth={2} name="Lucro" />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
      </div>

      {/* Indicadores */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-blue-500">
          <p className="text-sm font-medium text-gray-600">Receita Bruta</p>
          <p className="text-2xl font-bold text-blue-600">
            R$ {dreData?.receita_bruta.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-500">
          <p className="text-sm font-medium text-gray-600">Lucro Bruto</p>
          <p className="text-2xl font-bold text-green-600">
            R$ {dreData?.lucro_bruto.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-purple-500">
          <p className="text-sm font-medium text-gray-600">Lucro Líquido</p>
          <p className="text-2xl font-bold text-purple-600">
            R$ {dreData?.lucro_liquido.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-yellow-500">
          <p className="text-sm font-medium text-gray-600">Margem Líquida</p>
          <p className="text-2xl font-bold text-yellow-600">{dreData?.margem_liquida.toFixed(1)}%</p>
        </div>
      </div>
    </div>
  );
}
