import assert from 'node:assert/strict'
import { beforeEach, describe, it } from 'node:test'
import { createPinia, setActivePinia } from 'pinia'

import { useAuthStore } from '../authStore'

const values = new Map<string, string>()
const storage: Storage = {
  get length() {
    return values.size
  },
  clear: () => values.clear(),
  getItem: (key) => values.get(key) ?? null,
  key: (index) => Array.from(values.keys())[index] ?? null,
  removeItem: (key) => values.delete(key),
  setItem: (key, value) => values.set(key, value),
}

Object.defineProperty(globalThis, 'localStorage', {
  configurable: true,
  value: storage,
})

describe('auth session restoration', () => {
  beforeEach(() => {
    localStorage.clear()
    setActivePinia(createPinia())
  })

  it('restores a valid user and token', () => {
    const user = { id: 7, name: 'Ada Lovelace', email: 'ada@example.com', role: 'Admin' }
    localStorage.setItem('user', JSON.stringify(user))
    localStorage.setItem('token', 'valid-token')

    const store = useAuthStore()
    store.restoreSession()

    assert.deepEqual(store.currentUser, user)
    assert.equal(store.token, 'valid-token')
    assert.equal(store.isAuthenticated, true)
  })

  it('clears a malformed stored user instead of throwing', () => {
    localStorage.setItem('user', '{bad json')
    localStorage.setItem('token', 'stale-token')

    const store = useAuthStore()
    assert.doesNotThrow(() => store.restoreSession())

    assert.equal(store.currentUser, null)
    assert.equal(store.token, null)
    assert.equal(localStorage.getItem('user'), null)
    assert.equal(localStorage.getItem('token'), null)
  })

  it('rejects structurally invalid stored users', () => {
    localStorage.setItem('user', JSON.stringify({ name: 'Incomplete user' }))

    const store = useAuthStore()
    store.restoreSession()

    assert.equal(store.isAuthenticated, false)
    assert.equal(localStorage.getItem('user'), null)
  })

  it('removes an orphaned token when no user is stored', () => {
    localStorage.setItem('token', 'orphaned-token')

    const store = useAuthStore()
    store.restoreSession()

    assert.equal(store.token, null)
    assert.equal(localStorage.getItem('token'), null)
  })

  it('clears the previous token when a new user signs in without one', () => {
    const store = useAuthStore()
    store.setUser({ id: 1, name: 'Old User', email: 'old@example.com', role: 'User' }, 'old-token')
    const nextUser = { id: 2, name: 'New User', email: 'new@example.com', role: 'User' }

    store.setUser(nextUser)

    assert.deepEqual(store.currentUser, nextUser)
    assert.equal(store.isAuthenticated, true)
    assert.equal(store.token, null)
    assert.equal(localStorage.getItem('token'), null)
    assert.deepEqual(JSON.parse(localStorage.getItem('user')!), nextUser)

    setActivePinia(createPinia())
    const restored = useAuthStore()
    restored.restoreSession()
    assert.deepEqual(restored.currentUser, nextUser)
    assert.equal(restored.token, null)
  })

  it('replaces the previous token when a new token is supplied', () => {
    const store = useAuthStore()
    const user = { id: 1, name: 'Ada', email: 'ada@example.com', role: 'User' }
    store.setUser(user, 'old-token')

    store.setUser(user, 'new-token')

    assert.equal(store.token, 'new-token')
    assert.equal(localStorage.getItem('token'), 'new-token')
  })

  it('clears an old token when an empty token is supplied', () => {
    const store = useAuthStore()
    const user = { id: 1, name: 'Ada', email: 'ada@example.com', role: 'User' }
    store.setUser(user, 'old-token')

    store.setUser(user, '')

    assert.equal(store.token, null)
    assert.equal(localStorage.getItem('token'), null)
  })
})
