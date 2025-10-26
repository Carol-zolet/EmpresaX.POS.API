import { test, expect } from '@playwright/test';

const usuario = 'admin@empresa.com';
const senha = 'SenhaForte123';

// Ajuste a URL conforme necessário
const BASE_URL = 'http://localhost:3000';

test('Login e importação de planilha', async ({ page }) => {
  await page.goto(`${BASE_URL}/importacao-planilha`);

  // Deve redirecionar para login
  await expect(page.getByRole('heading', { name: 'Login' })).toBeVisible();

  // Preencher login
  await page.getByLabel('Email').fill(usuario);
  await page.getByLabel('Senha').fill(senha);
  await page.getByRole('button', { name: 'Entrar' }).click();

  // Após login, deve mostrar tela de importação
  await expect(page.getByText('Importar Planilha de Caixa')).toBeVisible();

  // Simular upload de planilha
  const filePath = 'tests/fixtures/planilha-caixa-valida.xlsx';
  await page.setInputFiles('input[type="file"]', filePath);
  await page.getByRole('button', { name: 'Importar' }).click();

  // Espera feedback de sucesso
  await expect(page.getByText('Resumo da Importação')).toBeVisible();
});
