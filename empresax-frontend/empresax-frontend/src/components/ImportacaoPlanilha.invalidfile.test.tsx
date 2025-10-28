import { render, screen, fireEvent } from '@testing-library/react';
import ImportacaoPlanilha from './ImportacaoPlanilha';
import { AuthProvider } from '../context/AuthContext';
import { LoadingProvider } from '../context/LoadingContext';

test('exibe erro ao importar arquivo não Excel', async () => {
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
