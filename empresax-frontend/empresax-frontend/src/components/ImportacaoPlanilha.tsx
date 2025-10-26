import React, { useRef, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { useLoading } from '../context/LoadingContext';

interface ImportacaoResult {
  sucesso: boolean;
  errosEncontrados: number;
  movimentosImportados: number;
  erros: { linha: number; mensagem: string }[];
}

const ImportacaoPlanilha: React.FC = () => {
  const fileInputRef = useRef<HTMLInputElement>(null);
  const { token } = useAuth();
  const { setLoading } = useLoading();
  const [progresso, setProgresso] = useState(0);
  const [resultado, setResultado] = useState<ImportacaoResult | null>(null);
  const [erro, setErro] = useState<string | null>(null);

  const handleUpload = async () => {
    setErro(null);
    setResultado(null);
    const file = fileInputRef.current?.files?.[0];
    if (!file) {
      setErro('Selecione um arquivo Excel (.xlsx ou .xls)');
      return;
    }
    const formData = new FormData();
    formData.append('arquivo', file);
    setLoading(true);
    setProgresso(10);
    try {
      const response = await fetch('/api/importacao/planilha-caixa', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData,
      });
      setProgresso(70);
      if (!response.ok) {
        const problem = await response.json();
        setErro(problem.detail || 'Erro ao importar planilha');
      } else {
        const data = await response.json();
        setResultado(data);
      }
    } catch (e) {
      setErro('Erro de rede ou servidor');
    } finally {
      setProgresso(100);
      setLoading(false);
    }
  };

  return (
    <div>
      <h2>Importar Planilha de Caixa</h2>
      <input
        type="file"
        accept=".xlsx,.xls"
        ref={fileInputRef}
        aria-label="Arquivo Excel"
      />
      <button onClick={handleUpload} aria-busy={progresso > 0 && progresso < 100}>
        Importar
      </button>
      {progresso > 0 && progresso < 100 && (
        <progress value={progresso} max={100} aria-valuenow={progresso} aria-valuemax={100} />
      )}
      {erro && <div role="alert" style={{ color: 'red' }}>{erro}</div>}
      {resultado && (
        <div>
          <h3>Resumo da Importação</h3>
          <p>Movimentos importados: {resultado.movimentosImportados}</p>
          <p>Erros encontrados: {resultado.errosEncontrados}</p>
          {resultado.erros.length > 0 && (
            <ul>
              {resultado.erros.map((e, i) => (
                <li key={i}>Linha {e.linha}: {e.mensagem}</li>
              ))}
            </ul>
          )}
        </div>
      )}
    </div>
  );
};

export default ImportacaoPlanilha;
