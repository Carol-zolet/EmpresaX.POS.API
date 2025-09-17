import { useState, useEffect } from "react";
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from "recharts";

interface ItemABC {
  nome: string;
  valor: number;
  percentual: number;
  percentualAcumulado: number;
  categoria: "A" | "B" | "C";
}

export default function CurvaABC() {
  const [clientes, setClientes] = useState<ItemABC[]>([]);
  const [produtos, setProdutos] = useState<ItemABC[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<"clientes" | "produtos">("clientes");

  useEffect(() => {
    // Dados mockados para Curva ABC
    const mockClientes = [
      { nome: "Cliente Premium Corp", valor: 85000, percentual: 35.4, percentualAcumulado: 35.4, categoria: "A" as const },
      { nome: "Empresa Tecnologia Ltda", valor: 65000, percentual: 27.1, percentualAcumulado: 62.5, categoria: "A" as const },
      { nome: "Comércio Global SA", valor: 45000, percentual: 18.8, percentualAcumulado: 81.3, categoria: "B" as const },
      { nome: "Indústria Nacional", valor: 25000, percentual: 10.4, percentualAcumulado: 91.7, categoria: "B" as const },
      { nome: "Pequena Empresa", valor: 12000, percentual: 5.0, percentualAcumulado: 96.7, categoria: "C" as const },
      { nome: "Outros Clientes", valor: 8000, percentual: 3.3, percentualAcumulado: 100.0, categoria: "C" as const },
    ];

    const mockProdutos = [
      { nome: "Notebook Premium", valor: 95000, percentual: 40.2, percentualAcumulado: 40.2, categoria: "A" as const },
      { nome: "Smartphone Top", valor: 75000, percentual: 31.7, percentualAcumulado: 71.9, categoria: "A" as const },
      { nome: "Tablet Pro", valor: 35000, percentual: 14.8, percentualAcumulado: 86.7, categoria: "B" as const },
      { nome: "Acessórios Premium", valor: 20000, percentual: 8.5, percentualAcumulado: 95.2, categoria: "B" as const },
      { nome: "Produtos Básicos", valor: 11500, percentual: 4.8, percentualAcumulado: 100.0, categoria: "C" as const },
    ];

    setTimeout(() => {
      setClientes(mockClientes);
      setProdutos(mockProdutos);
      setLoading(false);
    }, 1000);
  }, []);

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center">
        <div className="text-lg">Carregando Curva ABC...</div>
      </div>
    );
  }

  const currentData = activeTab === "clientes" ? clientes : produtos;
  const categoriaA = currentData.filter(item => item.categoria === "A");
  const categoriaB = currentData.filter(item => item.categoria === "B");
  const categoriaC = currentData.filter(item => item.categoria === "C");

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Curva ABC</h1>
        <div className="flex space-x-1 bg-gray-100 rounded-lg p-1">
          <button
            onClick={() => setActiveTab("clientes")}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              activeTab === "clientes"
                ? "bg-white text-blue-600 shadow-sm"
                : "text-gray-600 hover:text-gray-900"
            }`}
          >
            Clientes
          </button>
          <button
            onClick={() => setActiveTab("produtos")}
            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
              activeTab === "produtos"
                ? "bg-white text-blue-600 shadow-sm"
                : "text-gray-600 hover:text-gray-900"
            }`}
          >
            Produtos
          </button>
        </div>
      </div>

      {/* Resumo das Categorias */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-500">
          <p className="text-sm font-medium text-gray-600">Categoria A (Alta Importância)</p>
          <p className="text-2xl font-bold text-green-600">{categoriaA.length} itens</p>
          <p className="text-sm text-gray-500">
            R$ {categoriaA.reduce((sum, item) => sum + item.valor, 0).toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-yellow-500">
          <p className="text-sm font-medium text-gray-600">Categoria B (Média Importância)</p>
          <p className="text-2xl font-bold text-yellow-600">{categoriaB.length} itens</p>
          <p className="text-sm text-gray-500">
            R$ {categoriaB.reduce((sum, item) => sum + item.valor, 0).toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-red-500">
          <p className="text-sm font-medium text-gray-600">Categoria C (Baixa Importância)</p>
          <p className="text-2xl font-bold text-red-600">{categoriaC.length} itens</p>
          <p className="text-sm text-gray-500">
            R$ {categoriaC.reduce((sum, item) => sum + item.valor, 0).toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Gráfico */}
        <div className="bg-white shadow-lg rounded-xl p-6">
          <h3 className="text-lg font-semibold mb-4 text-gray-900">
            Distribuição ABC - {activeTab === "clientes" ? "Clientes" : "Produtos"}
          </h3>
          <div className="h-80">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={currentData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="nome" angle={-45} textAnchor="end" height={80} />
                <YAxis tickFormatter={(value) => `R$ ${(value/1000).toFixed(0)}k`} />
                <Tooltip formatter={(value: number) => `R$ ${value.toLocaleString('pt-BR', {minimumFractionDigits: 2})}`} />
                <Bar 
                  dataKey="valor" 
                  fill={(entry: any) => 
                    entry?.categoria === 'A' ? '#10b981' : 
                    entry?.categoria === 'B' ? '#f59e0b' : '#ef4444'
                  }
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Tabela */}
        <div className="bg-white shadow-lg rounded-xl p-6">
          <h3 className="text-lg font-semibold mb-4 text-gray-900">Detalhamento</h3>
          <div className="overflow-y-auto max-h-80">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50 sticky top-0">
                <tr>
                  <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Item</th>
                  <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Valor</th>
                  <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">%</th>
                  <th className="px-4 py-3 text-left text-xs font-medium text-gray-500 uppercase">Cat.</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {currentData.map((item, index) => (
                  <tr key={index} className="hover:bg-gray-50">
                    <td className="px-4 py-3 text-sm font-medium text-gray-900">{item.nome}</td>
                    <td className="px-4 py-3 text-sm text-gray-900">
                      R$ {item.valor.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                    </td>
                    <td className="px-4 py-3 text-sm text-gray-500">{item.percentual.toFixed(1)}%</td>
                    <td className="px-4 py-3 text-sm">
                      <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                        item.categoria === 'A' ? 'bg-green-100 text-green-800' :
                        item.categoria === 'B' ? 'bg-yellow-100 text-yellow-800' :
                        'bg-red-100 text-red-800'
                      }`}>
                        {item.categoria}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}
