import { test, expect } from '@playwright/test';

test('login e importação de planilha', async ({ page }) => {
  await page.goto('http://localhost:3000/importacao-planilha');
  await expect(page.getByText(/login/i)).toBeVisible();
  await page.getByLabel('Email').fill('admin@empresa.com');
  await page.getByLabel('Senha').fill('SenhaForte123');
  await page.getByRole('button', { name: 'Entrar' }).click();
  await expect(page.getByText(/importar planilha/i)).toBeVisible();
  await page.setInputFiles('input[type="file"]', 'tests/fixtures/planilha-caixa-valida.xlsx');
  await page.getByRole('button', { name: 'Importar' }).click();
  await expect(page.getByText(/resumo da importação/i)).toBeVisible();
});
