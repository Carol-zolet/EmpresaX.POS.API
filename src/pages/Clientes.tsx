import { useState, useEffect } from "react";

interface Cliente {
  id: number;
  nome: string;
  email: string;
  telefone?: string;
  totalCompras: number;
  ultimaCompra: string;
  status: "Ativo" | "Inativo";
}

export default function Clientes() {
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Dados mockados - substituir pela API real
    const mockClientes: Cliente[] = [
      { id: 1, nome: "João Silva", email: "joao@email.com", telefone: "(11) 99999-9999", totalCompras: 15780.50, ultimaCompra: "2025-09-01", status: "Ativo" },
      { id: 2, nome: "Maria Santos", email: "maria@email.com", telefone: "(11) 88888-8888", totalCompras: 8950.00, ultimaCompra: "2025-08-28", status: "Ativo" },
      { id: 3, nome: "Pedro Oliveira", email: "pedro@email.com", telefone: "(11) 77777-7777", totalCompras: 23400.75, ultimaCompra: "2025-09-05", status: "Ativo" },
      { id: 4, nome: "Ana Costa", email: "ana@email.com", totalCompras: 5680.00, ultimaCompra: "2025-07-15", status: "Inativo" },
      { id: 5, nome: "Carlos Lima", email: "carlos@email.com", telefone: "(11) 66666-6666", totalCompras: 12300.25, ultimaCompra: "2025-08-30", status: "Ativo" },
    ];
    
    setTimeout(() => {
      setClientes(mockClientes);
      setLoading(false);
    }, 1000);
  }, []);

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center">
        <div className="text-lg">Carregando clientes...</div>
      </div>
    );
  }

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Cadastro de Clientes</h1>
        <div className="text-sm text-gray-500">
          Total: {clientes.length} clientes
        </div>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-6">
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-500">
          <p className="text-sm font-medium text-gray-600">Clientes Ativos</p>
          <p className="text-2xl font-bold text-gray-900">{clientes.filter(c => c.status === 'Ativo').length}</p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-red-500">
          <p className="text-sm font-medium text-gray-600">Clientes Inativos</p>
          <p className="text-2xl font-bold text-gray-900">{clientes.filter(c => c.status === 'Inativo').length}</p>
        </div>
        <div className="bg-white shadow-lg rounded-xl p-6 border-l-4 border-blue-500">
          <p className="text-sm font-medium text-gray-600">Faturamento Total</p>
          <p className="text-2xl font-bold text-gray-900">
            R$ {clientes.reduce((sum, c) => sum + c.totalCompras, 0).toLocaleString('pt-BR', {minimumFractionDigits: 2})}
          </p>
        </div>
      </div>

      <div className="bg-white shadow-lg rounded-xl overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ID</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nome</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Contato</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Total Compras</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Última Compra</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {clientes.map((cliente) => (
              <tr key={cliente.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                  #{cliente.id.toString().padStart(3, '0')}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{cliente.nome}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  <div>{cliente.email}</div>
                  {cliente.telefone && <div className="text-xs text-gray-400">{cliente.telefone}</div>}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                  R$ {cliente.totalCompras.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {new Date(cliente.ultimaCompra).toLocaleDateString('pt-BR')}
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                    cliente.status === 'Ativo' ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
                  }`}>
                    {cliente.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
