import { useState, useEffect } from "react";

interface Produto {
  id: string;
  nome: string;
  categoria: string;
  custo: number;
  venda: number;
  estoque: number;
  vendidos: number;
}

export default function Produtos() {
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const mockProdutos: Produto[] = [
      { id: "P001", nome: "Notebook Dell", categoria: "Informática", custo: 2800.00, venda: 4200.00, estoque: 15, vendidos: 45 },
      { id: "P002", nome: "Mouse Logitech", categoria: "Acessórios", custo: 45.00, venda: 89.90, estoque: 120, vendidos: 280 },
      { id: "P003", nome: "Teclado Mecânico", categoria: "Acessórios", custo: 180.00, venda: 320.00, estoque: 35, vendidos: 95 },
      { id: "P004", nome: "Monitor 24\"", categoria: "Monitores", custo: 680.00, venda: 1200.00, estoque: 8, vendidos: 32 },
      { id: "P005", nome: "HD Externo 1TB", categoria: "Armazenamento", custo: 280.00, venda: 450.00, estoque: 25, vendidos: 78 },
    ];
    
    setTimeout(() => {
      setProdutos(mockProdutos);
      setLoading(false);
    }, 1000);
  }, []);

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center">
        <div className="text-lg">Carregando produtos...</div>
      </div>
    );
  }

  const totalVendasValor = produtos.reduce((sum, p) => sum + (p.venda * p.vendidos), 0);
  const totalCustos = produtos.reduce((sum, p) => sum + (p.custo * p.vendidos), 0);
  const margemGeral = totalVendasValor > 0 ? ((totalVendasValor - totalCustos) / totalVendasValor) * 100 : 0;

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Cadastro de Produtos</h1>
        <div className="text-sm text-gray-500">
          Total: {produtos.length} produtos
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-blue-500">
          <p className="text-sm font-medium text-gray-600">Total Produtos</p>
          <p className="text-2xl font-bold text-gray-900">{produtos.length}</p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-500">
          <p className="text-sm font-medium text-gray-600">Faturamento</p>
          <p className="text-2xl font-bold text-gray-900">R$ {totalVendasValor.toLocaleString('pt-BR', {minimumFractionDigits: 2})}</p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-yellow-500">
          <p className="text-sm font-medium text-gray-600">Custos</p>
          <p className="text-2xl font-bold text-gray-900">R$ {totalCustos.toLocaleString('pt-BR', {minimumFractionDigits: 2})}</p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-purple-500">
          <p className="text-sm font-medium text-gray-600">Margem Geral</p>
          <p className="text-2xl font-bold text-gray-900">{margemGeral.toFixed(1)}%</p>
        </div>
      </div>

      <div className="bg-white shadow-lg rounded-xl overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Produto</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Categoria</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Custo</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Venda</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Margem</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estoque</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vendidos</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {produtos.map((produto) => {
              const margem = ((produto.venda - produto.custo) / produto.venda) * 100;
              return (
                <tr key={produto.id} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{produto.id}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{produto.nome}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{produto.categoria}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    R$ {produto.custo.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    R$ {produto.venda.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                    <span className={`${margem > 50 ? 'text-green-600' : margem > 30 ? 'text-yellow-600' : 'text-red-600'}`}>
                      {margem.toFixed(1)}%
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                    <span className={`${produto.estoque < 10 ? 'text-red-600 font-bold' : 'text-gray-900'}`}>
                      {produto.estoque}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{produto.vendidos}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}
