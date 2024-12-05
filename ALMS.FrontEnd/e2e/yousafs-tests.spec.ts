import { test, expect } from '@playwright/test'

test('Use case 3', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('branchlibrarian@example.com');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('SecureP@ssw0rd');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await page.getByRole('link', { name: 'Inventory' }).click();
  await page.getByRole('button', { name: 'Add Media' }).click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').fill('test');
  await page.locator('textarea').click();
  await page.locator('textarea').fill('test');
  await page.locator('input[type="date"]').fill('2000-12-12');
  await page.locator('div').filter({ hasText: /^ISBN$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^ISBN$/ }).getByRole('textbox').fill('1234567');
  await page.locator('input[type="url"]').click();
  await page.locator('input[type="url"]').click();
  await page.locator('input[type="url"]').fill('https://picsum.photos/200/300');
  await page.locator('div').filter({ hasText: /^Author$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Author$/ }).getByRole('textbox').fill('test');
  await page.locator('div').filter({ hasText: /^Genre$/ }).getByRole('textbox').click({
    button: 'right'
  });
  await page.locator('div').filter({ hasText: /^Genre$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Genre$/ }).getByRole('textbox').fill('test');
  await page.getByRole('combobox').selectOption('1');
  await page.getByRole('button', { name: 'Create' }).click();
  await page.getByPlaceholder('Search inventory...').click();
  await page.getByPlaceholder('Search inventory...').fill('test');
  await expect(page.getByRole('cell', { name: 'test' }).first()).toBeVisible();
  await page.getByPlaceholder('Search inventory...').click();
  await page.getByPlaceholder('Search inventory...').fill('not made');
  await page.getByPlaceholder('Search inventory...').click();
  await page.getByPlaceholder('Search inventory...').fill('test');
  await page.getByRole('button').nth(2).click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').click();
  await page.locator('div').filter({ hasText: /^Title$/ }).getByRole('textbox').fill('test edited');
  await page.getByRole('button', { name: 'Update' }).click();
  await expect(page.getByRole('cell', { name: 'test edited' })).toBeVisible();
  await page.getByRole('button').nth(3).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
});

// add login 

// add register 

// add forgot password