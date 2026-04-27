import { defineStore } from 'pinia'
import { computed, ref } from 'vue'

interface User {
  id: number | null
  name: string
  email: string
  role: string
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

    if (savedUser) {
      currentUser.value = JSON.parse(savedUser)
    }

    if (savedToken) {
      token.value = savedToken
    }
  }

  return {
    currentUser, token,
    isAuthenticated, userName, userRole, avatarInitials,
    setUser, logout, restoreSession
  }
})
