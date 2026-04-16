<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

type LoginPayload = {
  email: string
  password: string
}

const router = useRouter()
const authStore = useAuthStore()

const form = reactive<LoginPayload>({
  email: '',
  password: '',
})

const loading = ref(false)
const error = ref('')

const canSubmit = computed(() => /\S+@\S+\.\S+/.test(form.email) && form.password.length > 0)

const login = async () => {
  error.value = ''

  if (!canSubmit.value) {
    error.value = 'Enter a valid email and password.'
    return
  }

  loading.value = true

  try {
    const apiBase = (import.meta.env.VITE_API_BASE_URL ?? '').toString()
    const response = await fetch(`${apiBase}/users/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form),
    })

    if (!response.ok) {
      const data = await response.json().catch(() => null)
      throw new Error(data?.detail || data?.message || 'Login failed.')
    }

    const data = await response.json().catch(() => null)
    const apiUser = data?.user ?? data

    authStore.setUser({
      id: apiUser?.id ?? null,
      name: apiUser?.name ?? form.email,
      email: apiUser?.email ?? form.email,
      role: apiUser?.role ?? 'User',
    })

    await router.push('/')
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Request failed.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <section class="login-page">
    <div class="card">
      <h1>Login</h1>
      <p class="subtitle">Sign in to your IntraLink account</p>

      <form @submit.prevent="login" class="form">
        <label>
          Email
          <input v-model="form.email" type="email" placeholder="you@example.com" autocomplete="email" />
        </label>

        <label>
          Password
          <input v-model="form.password" type="password" placeholder="Your password" autocomplete="current-password" />
        </label>

        <button :disabled="loading || !canSubmit" type="submit">
          {{ loading ? 'Signing in...' : 'Sign in' }}
        </button>
      </form>

      <p v-if="error" class="msg error">{{ error }}</p>

      <p class="footer">
        No account yet?
        <RouterLink to="/register">Create one</RouterLink>
      </p>
    </div>
  </section>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: 24px;
  background: #f7f9fc;
}

.card {
  width: 100%;
  max-width: 420px;
  background: #fff;
  border: 1px solid #e6ebf3;
  border-radius: 14px;
  padding: 24px;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.06);
}

h1 {
  margin: 0;
  font-size: 1.5rem;
}

.subtitle {
  margin: 8px 0 20px;
  color: #516177;
}

.form {
  display: grid;
  gap: 12px;
}

label {
  display: grid;
  gap: 6px;
  font-size: 0.95rem;
}

input {
  border: 1px solid #dbe3ee;
  border-radius: 10px;
  padding: 10px 12px;
  font-size: 0.95rem;
}

input:focus {
  outline: none;
  border-color: #4a7dff;
  box-shadow: 0 0 0 3px rgba(74, 125, 255, 0.14);
}

button {
  margin-top: 8px;
  border: 0;
  border-radius: 10px;
  padding: 11px 14px;
  background: #2f6bff;
  color: #fff;
  font-weight: 600;
  cursor: pointer;
}

button:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}

.msg {
  margin-top: 14px;
  padding: 10px 12px;
  border-radius: 10px;
  font-size: 0.9rem;
}

.error {
  background: #fff1f1;
  color: #a63131;
  border: 1px solid #ffd3d3;
}

.footer {
  margin-top: 14px;
  color: #516177;
  font-size: 0.9rem;
}

@media (max-width: 480px) {
  .card {
    padding: 18px;
  }
}
</style>
