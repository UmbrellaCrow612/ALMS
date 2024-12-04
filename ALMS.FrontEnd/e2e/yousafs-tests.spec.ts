import { test, expect } from '@playwright/test'

test('Use case 3', async ({ page }) => {
  await page.getByRole('link', { name: 'Inventory' }).click();
  await page.getByRole('button', { name: 'Add Media' }).click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').fill('oijwejd');
  await page.locator('textarea').click();
  await page.locator('textarea').fill('oidwojd');
  await page.locator('input[type="date"]').fill('1200-12-12');
  await page.locator('div').filter({ hasText: /^ISBN$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^ISBN$/ }).getByRole('textbox').fill('123456789');
  await page.locator('input[type="url"]').click();
  await page.locator('input[type="url"]').fill('https://picsum.photos/200/300');
  await page.locator('div').filter({ hasText: /^Author$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Author$/ }).getByRole('textbox').fill('dewwd');
  await page.locator('div').filter({ hasText: /^Genre$/ }).click();
  await page.locator('div').filter({ hasText: /^Genre$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Genre$/ }).getByRole('textbox').fill('wdewd');
  await page.getByRole('combobox').selectOption('2');
  await page.getByRole('button', { name: 'Create' }).click();
  await page.getByPlaceholder('Search inventory...').click();
  await page.getByPlaceholder('Search inventory...').fill('oi');
  await expect(page.getByRole('cell', { name: 'oijwejd' })).toBeVisible();
  await page.getByRole('button').nth(2).click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').fill('test man 12');
  await page.getByRole('button', { name: 'Update' }).click();
  await page.getByPlaceholder('Search inventory...').click();
  await page.getByPlaceholder('Search inventory...').fill('test');
  await expect(page.getByRole('cell', { name: 'test man' })).toBeVisible();
});