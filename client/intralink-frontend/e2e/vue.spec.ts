import { test, expect } from '@playwright/test'

test('visits the app root url', async ({ page }) => {
  await page.goto('/')

  await expect(page.getByText('IntraLink')).toBeVisible()
  await expect(page.getByRole('button', { name: 'Feed' })).toBeVisible()
  await expect(page.getByPlaceholder('Search people, teams, posts')).toBeVisible()
})
