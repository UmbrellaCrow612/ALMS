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


