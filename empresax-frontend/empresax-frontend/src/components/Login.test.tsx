import { render, screen, fireEvent } from '@testing-library/react';
import { AuthProvider } from '../context/AuthContext';
import Login from './Login';

test('exibe erro ao tentar login sem credenciais', async () => {
  render(
    <AuthProvider>
      <Login />
    </AuthProvider>
  );
  fireEvent.click(screen.getByText(/entrar/i));
  expect(await screen.findByRole('alert')).toBeInTheDocument();
});
