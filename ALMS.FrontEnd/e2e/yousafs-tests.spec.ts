import { test, expect } from '@playwright/test'

// See here how to get started:
// https://playwright.dev/docs/intro
test('Authentication - User registers ', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Register' }).click();
  await page.locator('body').press('ControlOrMeta+-');
  await page.getByLabel('Username').click();
  await page.getByLabel('Username').fill('YW__123');
  await page.getByLabel('First Name').click();
  await page.getByLabel('First Name').fill('You_saf');
  await page.getByLabel('Last Name').click();
  await page.getByLabel('Last Name').fill('ewfwfwfwefwfef');
  await page.getByLabel('Address', { exact: true }).click();
  await page.getByLabel('Address', { exact: true }).fill('wfwfwefw');
  await page.getByLabel('Phone Number').click();
  await page.getByLabel('Phone Number').fill('123456789');
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('c1010865@my.shu.ac.uk');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('Password@123');
  await page.getByRole('button', { name: 'Register' }).click();
})
