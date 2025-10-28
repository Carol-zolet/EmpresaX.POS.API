import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { AuthProvider } from '../context/AuthContext';
import { LoadingProvider } from '../context/LoadingContext';
import Login from './Login';
import ImportacaoPlanilha from './ImportacaoPlanilha';

describe('Fluxo de erros do sistema', () => {
  beforeEach(() => {
    jest.resetAllMocks();
  });

  test('login sem preencher campos', async () => {
    render(
      <AuthProvider>
        <Login />
      </AuthProvider>
    );
    fireEvent.click(screen.getByText(/entrar/i));
    expect(await screen.findByRole('alert')).toBeInTheDocument();
  });

  test('login com senha errada', async () => {
    global.fetch = jest.fn().mockResolvedValue({ ok: false, json: async () => ({ message: 'Usuário ou senha inválidos' }) });
    render(
      <AuthProvider>
        <Login />
      </AuthProvider>
    );
    fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'admin@empresa.com' } });
    fireEvent.change(screen.getByLabelText(/senha/i), { target: { value: 'errada' } });
    fireEvent.click(screen.getByText(/entrar/i));
    expect(await screen.findByRole('alert')).toHaveTextContent(/inválidos/i);
  });

  test('importação sem arquivo', async () => {
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
  fireEvent.click(screen.getByRole('button', { name: /importar/i }));
    expect(await screen.findByRole('alert')).toHaveTextContent(/nenhum arquivo/i);
  });

  test('importação com arquivo não Excel', async () => {
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const fileInput = screen.getByLabelText(/arquivo excel/i);
    const file = new File(['conteudo'], 'arquivo.txt', { type: 'text/plain' });
    fireEvent.change(fileInput, { target: { files: [file] } });
  fireEvent.click(screen.getByRole('button', { name: /importar/i }));
    expect(await screen.findByRole('alert')).toHaveTextContent(/apenas arquivos excel/i);
  });

  test('importação com arquivo muito grande', async () => {
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const fileInput = screen.getByLabelText(/arquivo excel/i);
    // Simula arquivo > 10MB
    const file = new File([new ArrayBuffer(11_000_000)], 'grande.xlsx', { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    Object.defineProperty(file, 'size', { value: 11_000_000 });
    fireEvent.change(fileInput, { target: { files: [file] } });
  fireEvent.click(screen.getByRole('button', { name: /importar/i }));
    expect(await screen.findByRole('alert')).toHaveTextContent(/não pode exceder 10MB/i);
  });

  test('importação com erro de rede', async () => {
    global.fetch = jest.fn().mockRejectedValue(new Error('Network error'));
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const fileInput = screen.getByLabelText(/arquivo excel/i);
    const file = new File(['dummy'], 'caixa.xlsx', { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    fireEvent.change(fileInput, { target: { files: [file] } });
  fireEvent.click(screen.getByRole('button', { name: /importar/i }));
    expect(await screen.findByRole('alert')).toHaveTextContent(/erro de rede/i);
  });

  test('importação com erro 500 do backend', async () => {
    global.fetch = jest.fn().mockResolvedValue({ ok: false, json: async () => ({ detail: 'Erro ao importar planilha' }) });
    render(
      <AuthProvider>
        <LoadingProvider>
          <ImportacaoPlanilha />
        </LoadingProvider>
      </AuthProvider>
    );
    const fileInput = screen.getByLabelText(/arquivo excel/i);
    const file = new File(['dummy'], 'caixa.xlsx', { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    fireEvent.change(fileInput, { target: { files: [file] } });
  fireEvent.click(screen.getByRole('button', { name: /importar/i }));
    expect(await screen.findByRole('alert')).toHaveTextContent(/erro ao importar/i);
  });
});
