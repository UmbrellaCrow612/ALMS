import { test, expect } from '@playwright/test';

// See here how to get started:
// https://playwright.dev/docs/intro
test('Use case one end to end test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('librarymember@example.com');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('MemberP@ssw0rd');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await page.getByRole('link', { name: 'Search' }).click();
  await page.getByPlaceholder('Search by title').click();
  await page.getByPlaceholder('Search by title').fill('med');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByPlaceholder('Search by title').click();
  await page.getByPlaceholder('Search by title').fill('');
  await page.getByRole('button', { name: 'Search' }).click();
  await page.getByRole('button', { name: 'View Details' }).first().click();
  await page.getByRole('link', { name: 'Search' }).click();
})


test('Use case two end to end test', async ({ page }) => {
  await page.goto('http://localhost:5173/');
})