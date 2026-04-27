<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()
const router = useRouter()

const profileFields = computed(() => [
  { label: 'Name', value: authStore.currentUser?.name || '-' },
  { label: 'Email', value: authStore.currentUser?.email || '-' },
  { label: 'Role', value: authStore.currentUser?.role || 'User' },
])

const exitProfile = async () => {
  authStore.logout()
  await router.push('/login')
}
</script>

<template>
  <section class="profile-page">
    <div class="profile-card">
      <div class="profile-header">
        <span class="avatar">{{ authStore.avatarInitials }}</span>
        <div>
          <h1>{{ authStore.userName }}</h1>
          <p>{{ authStore.userRole || 'User' }}</p>
        </div>
      </div>

      <div class="profile-grid">
        <div v-for="item in profileFields" :key="item.label" class="profile-item">
          <div class="label">{{ item.label }}</div>
          <div class="value">{{ item.value }}</div>
        </div>
      </div>

      <button class="exit-btn" type="button" @click="exitProfile">Exit</button>

      <p class="hint">This is your profile main page. You can add personal information editing here later.</p>
    </div>
  </section>
</template>

<style scoped>
.profile-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: 24px;
  background: #f5f7fb;
}

.profile-card {
  width: 100%;
  max-width: 760px;
  background: #fff;
  border: 1px solid #e6ebf3;
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.06);
}

.profile-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 18px;
}

.avatar {
  width: 52px;
  height: 52px;
  border-radius: 14px;
  display: grid;
  place-items: center;
  font-weight: 700;
  color: #fff;
  background: linear-gradient(135deg, #5f72ff, #8b9cff);
}

h1 {
  margin: 0;
  font-size: 1.35rem;
}

.profile-header p {
  margin: 4px 0 0;
  color: #5f6b7a;
}

.profile-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 12px;
}

.profile-item {
  border: 1px solid #e6ebf3;
  border-radius: 12px;
  padding: 12px;
  background: #fbfcff;
}

.label {
  font-size: 0.8rem;
  color: #68768a;
}

.value {
  margin-top: 6px;
  font-weight: 600;
}

.hint {
  margin-top: 16px;
  color: #68768a;
}

.exit-btn {
  margin-top: 14px;
  border: 1px solid #ffd3d3;
  background: #fff1f1;
  color: #a63131;
  border-radius: 10px;
  padding: 10px 14px;
  font-weight: 600;
  cursor: pointer;
}

@media (max-width: 640px) {
  .profile-grid {
    grid-template-columns: 1fr;
  }
}
</style>
