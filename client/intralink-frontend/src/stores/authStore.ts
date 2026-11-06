import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

interface User {
  id: number | null
  name: string
  email: string
  role: string
}

function isStoredUser(value: unknown): value is User {
  if (typeof value !== 'object' || value === null) return false

  const user = value as Record<string, unknown>
  return (
    (typeof user.id === 'number' || user.id === null) &&
    typeof user.name === 'string' &&
    typeof user.email === 'string' &&
    typeof user.role === 'string'
  )
}

export const useAuthStore = defineStore('auth', () => {
  // Состояние
  const currentUser = ref<User | null>(null)
  const token = ref<string | null>(null)

  // Вычисляемые значения
  const isAuthenticated = computed(() => currentUser.value !== null)

  const userName = computed(() => currentUser.value?.name ?? 'Guest')
  const userRole = computed(() => currentUser.value?.role ?? '')

  // Инициалы из имени — "Hamza Ait" → "HA"
  const avatarInitials = computed(() => {
    if (!currentUser.value) return '?'
    return currentUser.value.name
      .split(' ')
      .map(word => word[0])
      .join('')
      .toUpperCase()
      .slice(0, 2)
  })

  // Действия
  function setUser(user: User, userToken?: string) {
    currentUser.value = user
    localStorage.setItem('user', JSON.stringify(user))

    if (userToken) {
      token.value = userToken
      localStorage.setItem('token', userToken)
    } else {
      token.value = null
      localStorage.removeItem('token')
    }
  }

  function logout() {
    currentUser.value = null
    token.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }

  // Восстановление из localStorage при старте
  function restoreSession() {
    const savedUser = localStorage.getItem('user')
    const savedToken = localStorage.getItem('token')

    currentUser.value = null
    token.value = null

    if (!savedUser) {
      localStorage.removeItem('token')
      return
    }

    try {
      const parsedUser: unknown = JSON.parse(savedUser)
      if (!isStoredUser(parsedUser)) {
        throw new TypeError('Stored user has an invalid shape')
      }

      currentUser.value = parsedUser
      token.value = savedToken
    } catch {
      localStorage.removeItem('user')
      localStorage.removeItem('token')
    }
  }

  return {
    currentUser, token,
    isAuthenticated, userName, userRole, avatarInitials,
    setUser, logout, restoreSession
  }
})
