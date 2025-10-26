import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { AuthProvider } from '../context/AuthContext';
import { LoadingProvider } from '../context/LoadingContext';
import ImportacaoPlanilha from './ImportacaoPlanilha';

// Mock fetch global
beforeEach(() => {
  global.fetch = jest.fn();
});
afterEach(() => {
  jest.resetAllMocks();
});

describe('ImportacaoPlanilha', () => {
  it('exibe erro se nenhum arquivo for selecionado', async () => {
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    fireEvent.click(screen.getByText('Importar'));
    expect(await screen.findByRole('alert')).toHaveTextContent('Selecione um arquivo');
  });

  it('exibe resumo após upload bem-sucedido', async () => {
    (global.fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => ({
        sucesso: true,
        errosEncontrados: 0,
        movimentosImportados: 5,
        erros: []
      })
    });
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const file = new File(['dummy'], 'caixa.xlsx', { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const input = screen.getByLabelText('Arquivo Excel');
    fireEvent.change(input, { target: { files: [file] } });
    fireEvent.click(screen.getByText('Importar'));
    await waitFor(() => expect(screen.getByText('Resumo da Importação')).toBeInTheDocument());
    expect(screen.getByText('Movimentos importados: 5')).toBeInTheDocument();
  });

  it('exibe erro de API', async () => {
    (global.fetch as jest.Mock).mockResolvedValueOnce({
      ok: false,
      json: async () => ({ detail: 'Erro ao importar planilha' })
    });
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const file = new File(['dummy'], 'caixa.xlsx', { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const input = screen.getByLabelText('Arquivo Excel');
    fireEvent.change(input, { target: { files: [file] } });
    fireEvent.click(screen.getByText('Importar'));
    expect(await screen.findByRole('alert')).toHaveTextContent('Erro ao importar planilha');
  });
});
