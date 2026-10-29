import assert from 'node:assert/strict'
import { describe, it } from 'node:test'

import { getAuthRedirect, getPostLoginRedirect } from '../authGuard'

describe('authentication route guard', () => {
  it('redirects anonymous users from protected routes', () => {
    assert.deepEqual(getAuthRedirect(true, false, '/profile?tab=details'), {
      name: 'login',
      query: { redirect: '/profile?tab=details' },
    })
  })

  it('allows authenticated users to open protected routes', () => {
    assert.equal(getAuthRedirect(true, true, '/profile'), true)
  })

  it('allows anonymous users to open public routes', () => {
    assert.equal(getAuthRedirect(false, false, '/login'), true)
  })
})

describe('post-login redirect', () => {
  it('preserves a local application path', () => {
    assert.equal(getPostLoginRedirect('/profile?tab=details'), '/profile?tab=details')
  })

  it('rejects external and malformed redirect values', () => {
    assert.equal(getPostLoginRedirect('https://example.com'), '/')
    assert.equal(getPostLoginRedirect('//example.com'), '/')
    assert.equal(getPostLoginRedirect(['/profile']), '/')
    assert.equal(getPostLoginRedirect(null), '/')
  })
})
