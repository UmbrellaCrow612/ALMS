import { test, expect } from '@playwright/test';

// See here how to get started:
// https://playwright.dev/docs/intro
test('test for use case 1', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('branchlibrarian@example.com');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('SecureP@ssw0rd');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.getByPlaceholder('Search by title').click();
  await page.getByPlaceholder('Search by title').press('CapsLock');
  await page.getByPlaceholder('Search by title').fill('S');
  await page.getByPlaceholder('Search by title').press('CapsLock');
  await page.getByPlaceholder('Search by title').fill('Say');
  await page.getByPlaceholder('Search by title').press('Enter');
  await page.getByRole('button', { name: 'Search' }).click();
  await expect(page.getByRole('heading', { name: 'Sayings and anecdotes' })).toBeVisible();
  await page.getByPlaceholder('Search by title').click();
  await page.getByPlaceholder('Search by title').fill('gdbgdbd');
  await page.getByRole('button', { name: 'Search' }).click();
  await expect(page.getByText('No results found.')).toBeVisible();
});

test('test for use case 2', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('branchlibrarian@example.com');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('SecureP@ssw0rd');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('.mt-2').first().click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(2) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(3) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(4) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(5) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(6) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(7) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(8) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(9) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(10) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(11) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('div:nth-child(12) > .flex-1 > .mt-2').click();
  await page.getByRole('button', { name: 'Borrow' }).click();
  await page.getByRole('button', { name: 'Confirm' }).click();
  await expect(page.getByText('User has already borrowed')).toBeVisible();
  await page.getByRole('button', { name: 'Cancel' }).click();
  await page.getByRole('navigation').getByRole('button').click();
  await page.getByRole('link', { name: 'My Media' }).click();
  await expect(page.getByRole('cell', { name: 'Mediations' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Sayings and anecdotes' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Sayings and Sami' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Quantum Horizons' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Echoes of Empires' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Whispers of the Soul' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'The Digital Revolution' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Paris Nights' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Reflections on Existence' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'The Jade Cipher' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Realms of Enchantment' })).toBeVisible();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.locator('.mt-2').first().click();

});