<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

type RegisterPayload = {
  name: string
  email: string
  password: string
}

const router = useRouter()
const authStore = useAuthStore()

const form = reactive<RegisterPayload>({
  name: '',
  email: '',
  password: '',
})

const loading = ref(false)
const error = ref('')
const success = ref('')

const canSubmit = computed(() => {
  return form.name.trim().length > 1 && /\S+@\S+\.\S+/.test(form.email) && form.password.length >= 6
})

const register = async () => {
  error.value = ''
  success.value = ''

  if (!canSubmit.value) {
    error.value = 'Fill name, email, and password (min 6 chars).'
    return
  }

  loading.value = true

  try {
    const apiBase = (import.meta.env.VITE_API_BASE_URL ?? '').toString()
    const response = await fetch(`${apiBase}/users/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form),
    })

    if (!response.ok) {
      const data = await response.json().catch(() => null)
      throw new Error(data?.detail || data?.message || 'Registration failed.')
    }

    const data = await response.json().catch(() => null)
    const apiUser = data?.user ?? data
    const token = data?.token as string | undefined

    if (apiUser?.name && apiUser?.email) {
      authStore.setUser(
        {
          id: apiUser.id ?? null,
          name: apiUser.name,
          email: apiUser.email,
          role: apiUser.role ?? 'User',
        },
        token,
      )
    } else {
      authStore.setUser({
        id: null,
        name: form.name,
        email: form.email,
        role: 'User',
      })
    }

    success.value = 'Registration successful. Redirecting...'
    await router.push('/')
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Request failed.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <section class="register-page">
    <div class="card">
      <h1>Register</h1>
      <p class="subtitle">Create your IntraLink account</p>

      <form @submit.prevent="register" class="form">
        <label>
          Name
          <input v-model="form.name" type="text" placeholder="Your name" autocomplete="name" />
        </label>

        <label>
          Email
          <input v-model="form.email" type="email" placeholder="you@example.com" autocomplete="email" />
        </label>

        <label>
          Password
          <input v-model="form.password" type="password" placeholder="At least 6 characters" autocomplete="new-password" />
        </label>

        <button :disabled="loading || !canSubmit" type="submit">
          {{ loading ? 'Registering...' : 'Sign up' }}
        </button>
      </form>

      <p v-if="error" class="msg error">{{ error }}</p>
      <p v-if="success" class="msg success">{{ success }}</p>

      <p class="footer">
        Already have an account?
        <RouterLink to="/login">Sign in</RouterLink>
      </p>
    </div>
  </section>
</template>

<style scoped>
.register-page {
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

.success {
  background: #edfdf0;
  color: #1f6a30;
  border: 1px solid #c7f2cf;
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

