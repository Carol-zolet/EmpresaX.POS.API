import { useState } from "react";
import { CloudArrowUpIcon, DocumentTextIcon } from "@heroicons/react/24/outline";
import { useApiData } from "../hooks/useApiData";
import { ContaPagar } from "../types";
import Loading from "../components/Loading";

export default function ContasPagar() {
  const fallbackContas = [
    { id: 1, fornecedor: "Energia El?trica", descricao: "Conta de luz", valor: 450.30, vencimento: "2025-01-15", status: "Pendente", categoria: "Utilidades", createdAt: "2025-01-01" },
    { id: 2, fornecedor: "Internet Provider", descricao: "Internet fibra", valor: 120.00, vencimento: "2025-01-20", status: "Paga", categoria: "Telecomunica??es", createdAt: "2025-01-02" }
  ];

  const { data: response, loading, isOffline, refetch } = useApiData<{data: ContaPagar[]}>({
    url: 'http://localhost:5245/api/v1/financeiro/contas-pagar',
    fallbackData: { data: fallbackContas }
  });

  const [uploading, setUploading] = useState(false);
  const [message, setMessage] = useState<{type: 'success' | 'error', text: string} | null>(null);

  const contas = response?.data || [];

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
        refetch();
      } else {
        setMessage({ type: 'error', text: result.erro || 'Erro ao importar arquivo' });
      }
    } catch (error) {
      setMessage({ type: 'error', text: 'Erro ao conectar com a API' });
    }

    setUploading(false);
    event.target.value = '';
  };

  if (loading) {
    return <Loading message="Carregando contas a pagar..." />;
  }

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
        <h1 className="page-title">Contas a Pagar</h1>
        <div style={{ display: 'flex', gap: '1rem' }}>
          <button className="btn btn-primary">
            <DocumentTextIcon style={{ width: '1.25rem', height: '1.25rem' }} />
            Baixar Template
          </button>
          
          <label className="btn btn-success" style={{ cursor: 'pointer' }}>
            <CloudArrowUpIcon style={{ width: '1.25rem', height: '1.25rem' }} />
            {uploading ? 'Importando...' : 'Importar Excel'}
            <input
              type="file"
              accept=".xlsx,.xls"
              onChange={handleFileUpload}
              style={{ display: 'none' }}
              disabled={uploading}
            />
          </label>
        </div>
      </div>

      {isOffline && (
        <div className="card" style={{ background: '#fed7d7', borderColor: '#e53e3e' }}>
          <p style={{ color: '#742a2a', margin: 0 }}>API offline - exibindo dados de exemplo</p>
        </div>
      )}

      {message && (
        <div className={`card ${message.type === 'success' ? 'status-success' : 'status-error'}`} style={{ marginBottom: '1.5rem' }}>
          <p style={{ margin: 0 }}>{message.text}</p>
        </div>
      )}

      <div className="table-container">
        <table className="table">
          <thead>
            <tr>
              <th>Fornecedor</th>
              <th>Descri??o</th>
              <th>Valor</th>
              <th>Vencimento</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {contas.map((conta) => (
              <tr key={conta.id}>
                <td style={{ fontWeight: 500 }}>{conta.fornecedor}</td>
                <td>{conta.descricao}</td>
                <td style={{ fontWeight: 500 }}>
                  R$ {conta.valor.toLocaleString('pt-BR', {minimumFractionDigits: 2})}
                </td>
                <td>{new Date(conta.vencimento).toLocaleDateString('pt-BR')}</td>
                <td>
                  <span className={`status-badge ${
                    conta.status === 'Paga' ? 'status-success' :
                    conta.status === 'Pendente' ? 'status-warning' :
                    'status-error'
                  }`}>
                    {conta.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        
        {contas.length === 0 && (
          <div style={{ textAlign: 'center', padding: '3rem', color: '#718096' }}>
            <div style={{ fontSize: '3rem', marginBottom: '1rem' }}>??</div>
            <h3>Nenhuma conta cadastrada</h3>
            <p>Importe um arquivo Excel para come?ar</p>
          </div>
        )}
      </div>
    </div>
  );
}
