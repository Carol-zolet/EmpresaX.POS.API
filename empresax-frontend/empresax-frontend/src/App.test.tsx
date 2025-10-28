import React from 'react';

import { render, screen } from '@testing-library/react';
import App from './App';

test('renderiza tela de login', () => {
  render(<App />);
  expect(screen.getByText(/login/i)).toBeInTheDocument();
});
