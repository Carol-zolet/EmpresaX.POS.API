import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { AuthProvider } from '../context/AuthContext';
import Login from './Login';

test('exibe erro ao tentar login inválido', async () => {
  // Mock fetch para simular erro de autenticação
  global.fetch = jest.fn().mockResolvedValue({
    ok: false,
    json: async () => ({ message: 'Usuário ou senha inválidos' })
  });
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
