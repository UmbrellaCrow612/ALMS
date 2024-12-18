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

// checks that user who dose not exist cannot login and a user who dose can
test('Use case 4 - Login', async ({ page }) => {
  await page.goto('http://localhost:5173/login');
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('randomemail@gmail.com');
  await page.getByLabel('Email address').press('Tab');
  await page.getByLabel('Password').fill('bfvirewfibewfib');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await expect(page.getByText('User does not exist or is not')).toBeVisible();
  await page.getByLabel('Email address').click();
  await page.getByLabel('Email address').fill('branchlibrarian@example.com');
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('SecureP@ssw0rd');
  await page.getByRole('button', { name: 'Sign in' }).click();
  await expect(page.getByRole('navigation').getByRole('button')).toBeVisible();
});


// trys to register a user with invalid credential format and a user with correct credential format.
test('Use case 4 - User Registration', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Register' }).click();
  await page.getByLabel('Username').click();
  await page.getByLabel('Username').fill('dnwifnmwnf');
  await page.getByLabel('Username').press('Tab');
  await page.getByLabel('First Name').fill('wenfokwfoin');
  await page.getByLabel('First Name').press('Tab');
  await page.getByLabel('Last Name').fill('wenfoewfin');
  await page.getByLabel('Last Name').press('Tab');
  await page.getByLabel('Address', { exact: true }).fill('owenfogfni');
  await page.getByLabel('Address', { exact: true }).press('Tab');
  await page.getByLabel('Phone Number').fill('oiewfinfin');
  await page.getByLabel('Phone Number').press('Tab');
  await page.getByLabel('Email address').fill('oewnfoinf@gmail.com');
  await page.getByLabel('Email address').press('Tab');
  await page.getByLabel('Password').fill('oiewnfoinf');
  await page.getByRole('button', { name: 'Register' }).click();
  await expect(page.getByText('The PhoneNumber field is not')).toBeVisible();
  await page.getByLabel('Phone Number').click();
  await page.getByLabel('Phone Number').fill('123456789');
  await page.getByRole('button', { name: 'Register' }).click();
  await expect(page.getByText('PasswordRequiresNonAlphanumeric')).toBeVisible();
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('Password@123');
  await page.getByRole('button', { name: 'Register' }).click();
  await expect(page.getByText('You successfully created an')).toBeVisible();
});

// user gets a reset link
test('use case 4 - User clicks forgot password', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('button', { name: 'Login' }).click();
  await page.getByRole('link', { name: 'Forgot your password?' }).click();
  await page.getByLabel('Email Address').click();
  await page.getByLabel('Email Address').fill('weffwfw@gmail.com');
  await page.getByRole('button', { name: 'Send Reset Link' }).click();
  await expect(page.getByText('A password reset link has')).toBeVisible();
});