import { useState, useEffect } from "react";
import { CloudArrowUpIcon, DocumentTextIcon } from "@heroicons/react/24/outline";

interface ContaPagar {
  id: number;
  fornecedor: string;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  categoria: string;
  createdAt: string;
}

export default function ContasPagar() {
  const [contas, setContas] = useState<ContaPagar[]>([]);
  const [loading, setLoading] = useState(true);
  const [uploading, setUploading] = useState(false);
  const [message, setMessage] = useState<{type: 'success' | 'error', text: string} | null>(null);

  const fetchContas = async () => {
    try {
      const response = await fetch('http://localhost:5245/api/v1/financeiro/contas-pagar');
      if (response.ok) {
        const data = await response.json();
        setContas(data.data || []);
      }
    } catch (error) {
      console.error('Erro ao carregar contas:', error);
      // Dados mockados
      setContas([
        { id: 1, fornecedor: "Energia Elétrica", descricao: "Conta de luz", valor: 450.30, vencimento: "2025-10-15", status: "Pendente", categoria: "Utilidades", createdAt: "2025-09-01" },
        { id: 2, fornecedor: "Internet Provider", descricao: "Internet fibra", valor: 120.00, vencimento: "2025-10-20", status: "Paga", categoria: "Telecomunicações", createdAt: "2025-09-02" }
      ]);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchContas();
  }, []);

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    setUploading(true);
    setMessage(null);

    const formData = new FormData();
    formData.append('arquivo', file);

    try {
      const response = await fetch('http://localhost:5245/api/v1/financeiro/contas-pagar/importar-excel', {
        method: 'POST',
        body: formData
      });

      const result = await response.json();

      if (response.ok && result.sucesso) {
        setMessage({ type: 'success', text: result.mensagem });
        fetchContas(); // Recarregar dados
      } else {
        setMessage({ type: 'error', text: result.erro || 'Erro ao importar arquivo' });
      }
    } catch (error) {
      setMessage({ type: 'error', text: 'Erro ao conectar com a API' });
    }

    setUploading(false);
    event.target.value = ''; // Reset input
  };

  const downloadTemplate = async () => {
    try {
      const response = await fetch('http://localhost:5245/api/v1/financeiro/contas-pagar/template');
      if (response.ok) {
        const blob = await response.blob();
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'template_contas_pagar.csv';
        a.click();
        window.URL.revokeObjectURL(url);
      }
    } catch (error) {
      console.error('Erro ao baixar template:', error);
    }
  };

  if (loading) {
    return (
      <div className="ml-72 p-6 flex justify-center items-center">
        <div className="text-lg">Carregando contas a pagar...</div>
      </div>
    );
  }

  return (
    <div className="ml-72 p-6 space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900">Contas a Pagar</h1>
        <div className="flex space-x-3">
          <button
            onClick={downloadTemplate}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 flex items-center space-x-2"
          >
            <DocumentTextIcon className="h-5 w-5" />
            <span>Baixar Template</span>
          </button>
          
          <label className="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 cursor-pointer flex items-center space-x-2">
            <CloudArrowUpIcon className="h-5 w-5" />
            <span>{uploading ? 'Importando...' : 'Importar Excel'}</span>
            <input
              type="file"
              accept=".xlsx"
              onChange={handleFileUpload}
              className="hidden"
              disabled={uploading}
            />
          </label>
        </div>
      </div>

      {message && (
        <div className={`p-4 rounded-lg ${
          message.type === 'success' ? 'bg-green-100 text-green-700 border border-green-200' : 'bg-red-100 text-red-700 border border-red-200'
        }`}>
          {message.text}
        </div>
      )}

      <div className="bg-white shadow-lg rounded-xl overflow-hidden">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Fornecedor</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Descrição</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Valor</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vencimento</th>
              <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {contas.map((conta) => (
              <tr key={conta.id} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{conta.fornecedor}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{conta.descricao}</td>
                <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">
                  R$ {conta.valor.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                  {new Date(conta.vencimento).toLocaleDateString('pt-BR')}
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <span className={`inline-flex px-2 py-1 text-xs font-semibold rounded-full ${
                    conta.status === 'Paga' ? 'bg-green-100 text-green-800' :
                    conta.status === 'Pendente' ? 'bg-yellow-100 text-yellow-800' :
                    'bg-red-100 text-red-800'
                  }`}>
                    {conta.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        
        {contas.length === 0 && (
          <div className="text-center py-12 text-gray-500">
            Nenhuma conta cadastrada. Importe um arquivo Excel para começar.
          </div>
        )}
      </div>
    </div>
  );
}
