import assert from 'node:assert/strict'
import { describe, it } from 'node:test'

import { parseAuthResponse } from '../authResponse'

describe('authentication response parsing', () => {
  it('accepts the direct user response returned by the API', () => {
    assert.deepEqual(parseAuthResponse({
      id: 7,
      name: '  Ada Lovelace  ',
      email: '  ada@example.com  ',
    }), {
      user: {
        id: 7,
        name: 'Ada Lovelace',
        email: 'ada@example.com',
        role: 'User',
      },
    })
  })

  it('accepts an enveloped user and optional token', () => {
    assert.deepEqual(parseAuthResponse({
      user: {
        id: 9,
        name: 'Grace Hopper',
        email: 'grace@example.com',
        role: 'Admin',
      },
      token: 'session-token',
    }), {
      user: {
        id: 9,
        name: 'Grace Hopper',
        email: 'grace@example.com',
        role: 'Admin',
      },
      token: 'session-token',
    })
  })

  it('rejects missing or invalid user identifiers', () => {
    assert.equal(parseAuthResponse(null), null)
    assert.equal(parseAuthResponse({ name: 'Ada', email: 'ada@example.com' }), null)
    assert.equal(parseAuthResponse({ id: 0, name: 'Ada', email: 'ada@example.com' }), null)
    assert.equal(parseAuthResponse({ id: 1.5, name: 'Ada', email: 'ada@example.com' }), null)
    assert.equal(parseAuthResponse({ user: null, id: 1, name: 'Ada', email: 'ada@example.com' }), null)
  })

  it('rejects blank names and malformed email values', () => {
    assert.equal(parseAuthResponse({ id: 1, name: '  ', email: 'ada@example.com' }), null)
    assert.equal(parseAuthResponse({ id: 1, name: 'Ada', email: 'not-an-email' }), null)
    assert.equal(parseAuthResponse({ id: 1, name: ['Ada'], email: 'ada@example.com' }), null)
  })

  it('ignores blank optional tokens instead of persisting them', () => {
    assert.deepEqual(parseAuthResponse({
      user: { id: 1, name: 'Ada', email: 'ada@example.com' },
      token: '   ',
    }), {
      user: { id: 1, name: 'Ada', email: 'ada@example.com', role: 'User' },
    })
  })
})
