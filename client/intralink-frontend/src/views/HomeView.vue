<script setup lang="ts">
import { useAuthStore } from '@/stores/authStore'

const authStore = useAuthStore()

type NavIcon = 'feed' | 'chat' | 'profile' | 'teams' | 'events' | 'files' | 'settings'

const navItems: Array<{ label: string; icon: NavIcon; active?: boolean }> = [
  { label: 'Feed', icon: 'feed', active: true },
  { label: 'Chat', icon: 'chat' },
  { label: 'Profile', icon: 'profile' },
  { label: 'Teams', icon: 'teams' },
  { label: 'Events', icon: 'events' },
  { label: 'Files', icon: 'files' },
  { label: 'Settings', icon: 'settings' },
]

const navIconPath: Record<NavIcon, string> = {
  feed: 'M3 10.5L12 3l9 7.5V20a1 1 0 0 1-1 1h-5v-7H9v7H4a1 1 0 0 1-1-1v-9.5z',
  chat: 'M4 5h16a1 1 0 0 1 1 1v10a1 1 0 0 1-1 1H9l-5 4v-4H4a1 1 0 0 1-1-1V6a1 1 0 0 1 1-1z',
  profile: 'M12 12a4 4 0 1 0 0-8 4 4 0 0 0 0 8zm-7 8a7 7 0 0 1 14 0',
  teams: 'M8 11a3 3 0 1 0 0-6 3 3 0 0 0 0 6zm8 0a3 3 0 1 0 0-6 3 3 0 0 0 0 6zM2.5 20a5.5 5.5 0 0 1 11 0m3 0a5.5 5.5 0 0 1 5.5-5.5',
  events: 'M8 3v3m8-3v3M3 10h18M6 5h12a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V7a2 2 0 0 1 2-2z',
  files: 'M14 2H7a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V7l-5-5zm0 0v5h5',
  settings:
    'M12 9.2a2.8 2.8 0 1 0 0 5.6 2.8 2.8 0 0 0 0-5.6zm7.2 2.8-.9.5.1 1.1.9.6-1.4 2.4-1-.4-.9.6-.1 1.1h-2.8l-.1-1.1-1-.6-1 .4-1.4-2.4.9-.6.1-1.1-.9-.5 1.4-2.4 1 .4 1-.6.1-1.1h2.8l.1 1.1.9.6 1-.4 1.4 2.4z',
}

const friends = [
  { name: 'Emily Carter', role: 'Product Designer', status: 'online', color: '#7b8cff' },
  { name: 'Fiona Park', role: 'Data Analyst', status: 'offline', color: '#4fc3f7' },
  { name: 'Jennifer Lee', role: 'client Dev', status: 'online', color: '#f6b93b' },
  { name: 'Anne Rogers', role: 'HR Manager', status: 'away', color: '#ff7a90' },
  { name: 'Andrew Miles', role: 'Project Lead', status: 'online', color: '#5cd18a' },
  { name: 'Sonia Patel', role: 'Marketing', status: 'offline', color: '#7ed6df' },
]

const posts = [
  {
    id: 1,
    author: 'Amanda Rose',
    role: 'Growth Team',
    time: '2h ago',
    text: 'Q2 roadmap is live. Please review your team goals and add any blockers by Friday.',
    media: 'field',
    likes: 89,
    comments: 12,
    shares: 4,
  },
  {
    id: 2,
    author: 'Casie Nolan',
    role: 'Operations',
    time: '4h ago',
    text: 'Great energy from the onboarding cohort today. Welcome to the new joiners!',
    media: 'forest',
    likes: 56,
    comments: 9,
    shares: 2,
  },
  {
    id: 3,
    author: 'Mika Romanov',
    role: 'Engineering',
    time: '6h ago',
    text: 'Reminder: security patch window scheduled for Saturday 02:00-04:00 UTC.',
    media: 'lake',
    likes: 24,
    comments: 6,
    shares: 1,
  },
]

const notifications = [
  { title: 'Hally uploaded photos to "All Hands"', time: '1 hour ago' },
  { title: 'Jennifer sent you a friend request', time: '6 hours ago' },
  { title: 'Design team posted new brand assets', time: 'Yesterday' },
]

const communities = [
  { name: 'UI/UX Designers', members: '54 new posts' },
  { name: 'client Developers', members: '18 new posts' },
  { name: 'Product Ops', members: '7 new posts' },
]

const initials = (name: string) =>
  name
    .split(' ')
    .map((part) => part[0])
    .join('')
    .slice(0, 2)
    .toUpperCase()
</script>

<template>
  <div class="layout">
    <aside class="sidebar card">
      <div class="brand">
        <div class="brand-mark">IL</div>
        <div>
          <div class="brand-name">IntraLink</div>
          <div class="brand-subtitle">Corporate Social</div>
        </div>
      </div>

      <nav class="nav">
        <button
          v-for="item in navItems"
          :key="item.label"
          class="nav-item"
          :class="{ 'is-active': item.active }"
        >
          <svg class="nav-icon" viewBox="0 0 24 24" fill="none" aria-hidden="true">
            <path :d="navIconPath[item.icon]" />
          </svg>
          {{ item.label }}
        </button>
      </nav>

      <div class="sidebar-cta">
        <div class="cta-title">Create space</div>
        <p class="cta-text">Start a project hub for your team.</p>
        <button class="cta-button">New space</button>
      </div>
    </aside>

    <main class="main">
      <header class="topbar card">
        <div class="topbar-left">
          <button class="icon-btn">Menu</button>
          <label class="search">
            <span class="search-icon">Search</span>
            <input type="text" placeholder="Search people, teams, posts" />
          </label>
        </div>
        <div class="topbar-right">
          <button class="icon-btn">New</button>
          <button class="icon-btn">Alerts</button>
          <div class="user-chip">
            <span class="avatar avatar--sm">{{ authStore.avatarInitials }}</span>
            <div>
              <div class="user-name">{{ authStore.userName }}</div>
              <div class="user-role">{{ authStore.userRole || 'User' }}</div>
              <RouterLink to="/profile" class="ghost-button" style="margin-top: 4px; padding: 4px 8px; font-size: 0.75rem">
                View profile
              </RouterLink>
            </div>
          </div>
        </div>
      </header>

      <div class="content-grid">
        <section class="friends-panel card" style="--delay: 60ms">
          <div class="panel-header">
            <h2>Friends</h2>
            <button class="ghost-button">See all</button>
          </div>
          <div class="friends-list">
            <div v-for="friend in friends" :key="friend.name" class="friend-row">
              <span class="avatar" :style="{ '--avatar': friend.color }">
                {{ initials(friend.name) }}
              </span>
              <div class="friend-meta">
                <div class="friend-name">{{ friend.name }}</div>
                <div class="friend-role">{{ friend.role }}</div>
              </div>
              <span class="status" :data-status="friend.status" />
            </div>
          </div>
        </section>

        <section class="feed">
          <div class="composer card" style="--delay: 120ms">
            <div class="composer-top">
              <span class="avatar avatar--sm">{{ authStore.avatarInitials }}</span>
              <input class="composer-input" placeholder="Share an update with your team" />
            </div>
            <div class="composer-actions">
              <button class="ghost-button">Photo</button>
              <button class="ghost-button">Event</button>
              <button class="primary-button">Post</button>
            </div>
          </div>

          <article
            v-for="post in posts"
            :key="post.id"
            class="post card"
            :style="{ '--delay': `${post.id * 80}ms` }"
          >
            <header class="post-header">
              <div class="post-author">
                <span class="avatar avatar--sm">{{ initials(post.author) }}</span>
                <div>
                  <div class="post-name">{{ post.author }}</div>
                  <div class="post-role">{{ post.role }} - {{ post.time }}</div>
                </div>
              </div>
              <button class="ghost-button">More</button>
            </header>
            <p class="post-text">{{ post.text }}</p>
            <div class="media" :class="`media--${post.media}`"></div>
            <footer class="post-footer">
              <span>{{ post.likes }} likes</span>
              <span>{{ post.comments }} comments</span>
              <span>{{ post.shares }} shares</span>
            </footer>
          </article>
        </section>

        <aside class="rightbar">
          <div class="card right-card" style="--delay: 140ms">
            <div class="panel-header">
              <h2>Birthday</h2>
              <span class="badge">Today</span>
            </div>
            <p class="muted">Bernadette and 5 others have birthdays today.</p>
            <button class="primary-button full">Send wishes</button>
          </div>

          <div class="card right-card" style="--delay: 220ms">
            <div class="panel-header">
              <h2>Notifications</h2>
            </div>
            <div class="stack">
              <div v-for="note in notifications" :key="note.title" class="note-row">
                <div class="note-dot" />
                <div>
                  <div class="note-title">{{ note.title }}</div>
                  <div class="note-time">{{ note.time }}</div>
                </div>
              </div>
            </div>
          </div>

          <div class="card right-card" style="--delay: 300ms">
            <div class="panel-header">
              <h2>Communities</h2>
              <button class="ghost-button">Explore</button>
            </div>
            <div class="stack">
              <div v-for="community in communities" :key="community.name" class="community-row">
                <div class="community-icon" />
                <div>
                  <div class="community-name">{{ community.name }}</div>
                  <div class="community-meta">{{ community.members }}</div>
                </div>
              </div>
            </div>
          </div>
        </aside>
      </div>
    </main>
  </div>
</template>

<style scoped>
.layout {
  display: grid;
  grid-template-columns: 260px 1fr;
  gap: 24px;
  padding: 28px;
}

.card {
  background: var(--surface-100);
  border: 1px solid var(--line-100);
  border-radius: var(--radius-2);
  box-shadow: var(--shadow-1);
  animation: rise 640ms ease both;
  animation-delay: var(--delay, 0ms);
}

.sidebar {
  display: flex;
  flex-direction: column;
  gap: 24px;
  padding: 24px;
  position: sticky;
  top: 24px;
  align-self: start;
}

.brand {
  display: flex;
  align-items: center;
  gap: 12px;
}

.brand-mark {
  width: 44px;
  height: 44px;
  border-radius: 14px;
  background: linear-gradient(135deg, var(--accent-500), var(--mint-500));
  color: #fff;
  display: grid;
  place-items: center;
  font-family: 'Space Grotesk', system-ui, sans-serif;
  font-weight: 700;
}

.brand-name {
  font-family: 'Space Grotesk', system-ui, sans-serif;
  font-weight: 700;
  font-size: 1.1rem;
}

.brand-subtitle {
  color: var(--ink-500);
  font-size: 0.85rem;
}

.nav {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.nav-item {
  border: 0;
  background: transparent;
  text-align: left;
  padding: 10px 12px;
  border-radius: 12px;
  color: var(--ink-700);
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
}

.nav-item.is-active {
  background: var(--surface-200);
  color: var(--ink-900);
}

.nav-icon {
  width: 16px;
  height: 16px;
  stroke: currentColor;
  stroke-width: 1.8;
  stroke-linecap: round;
  stroke-linejoin: round;
  flex: 0 0 16px;
}

.sidebar-cta {
  background: linear-gradient(135deg, #f1f5ff, #fef6ea);
  padding: 16px;
  border-radius: var(--radius-1);
  border: 1px solid rgba(47, 128, 237, 0.12);
}

.cta-title {
  font-weight: 600;
  margin-bottom: 6px;
}

.cta-text {
  color: var(--ink-500);
  font-size: 0.9rem;
  margin-bottom: 12px;
}

.cta-button {
  border: 0;
  background: var(--accent-500);
  color: #fff;
  padding: 10px 14px;
  border-radius: 12px;
  font-weight: 600;
  cursor: pointer;
}

.main {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.topbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px;
  gap: 16px;
}

.topbar-left,
.topbar-right {
  display: flex;
  align-items: center;
  gap: 12px;
}

.icon-btn {
  border: 1px solid var(--line-100);
  background: var(--surface-100);
  padding: 8px 12px;
  border-radius: 999px;
  font-weight: 600;
  cursor: pointer;
}

.search {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--surface-200);
  padding: 8px 14px;
  border-radius: 999px;
  min-width: 280px;
}

.search-icon {
  color: var(--ink-500);
  font-size: 0.9rem;
}

.search input {
  border: 0;
  background: transparent;
  outline: none;
  width: 100%;
  font-size: 0.95rem;
}

.user-chip {
  display: flex;
  align-items: center;
  gap: 10px;
  background: var(--surface-200);
  padding: 6px 10px;
  border-radius: 999px;
}

.user-name {
  font-weight: 600;
  font-size: 0.9rem;
}

.user-role {
  font-size: 0.75rem;
  color: var(--ink-500);
}

.content-grid {
  display: grid;
  grid-template-columns: 260px minmax(0, 1fr) 280px;
  gap: 24px;
  align-items: start;
}

.panel-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
}

.panel-header h2 {
  font-size: 1.05rem;
  font-weight: 700;
}

.friends-panel {
  padding: 20px;
}

.friends-list {
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.friend-row {
  display: flex;
  align-items: center;
  gap: 12px;
}

.friend-meta {
  flex: 1;
}

.friend-name {
  font-weight: 600;
  font-size: 0.95rem;
}

.friend-role {
  color: var(--ink-500);
  font-size: 0.8rem;
}

.status {
  width: 8px;
  height: 8px;
  border-radius: 999px;
  background: var(--surface-300);
}

.status[data-status='online'] {
  background: var(--mint-500);
}

.status[data-status='away'] {
  background: var(--peach-500);
}

.avatar {
  width: 42px;
  height: 42px;
  border-radius: 14px;
  display: grid;
  place-items: center;
  font-weight: 600;
  color: #fff;
  background: var(--avatar, linear-gradient(135deg, #5f72ff, #8b9cff));
}

.avatar--sm {
  width: 34px;
  height: 34px;
  font-size: 0.75rem;
  border-radius: 12px;
}

.feed {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.composer {
  padding: 18px;
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.composer-top {
  display: flex;
  align-items: center;
  gap: 12px;
}

.composer-input {
  border: 0;
  background: var(--surface-200);
  padding: 10px 14px;
  border-radius: 14px;
  width: 100%;
  font-size: 0.95rem;
}

.composer-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.ghost-button {
  border: 1px solid var(--line-100);
  background: transparent;
  padding: 8px 12px;
  border-radius: 999px;
  cursor: pointer;
  font-weight: 600;
  color: var(--ink-700);
}

.primary-button {
  border: 0;
  background: var(--accent-500);
  color: #fff;
  padding: 8px 16px;
  border-radius: 999px;
  font-weight: 600;
  cursor: pointer;
}

.primary-button.full {
  width: 100%;
  margin-top: 12px;
}

.post {
  padding: 20px;
  display: flex;
  flex-direction: column;
  gap: 14px;
}

.post-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.post-author {
  display: flex;
  align-items: center;
  gap: 12px;
}

.post-name {
  font-weight: 600;
}

.post-role {
  color: var(--ink-500);
  font-size: 0.85rem;
}

.post-text {
  color: var(--ink-700);
}

.media {
  height: 180px;
  border-radius: var(--radius-1);
  background: linear-gradient(120deg, #dfe7ff, #f5f2ff);
}

.media--field {
  background: linear-gradient(120deg, #cde9d9, #f6f2c7);
}

.media--forest {
  background: linear-gradient(120deg, #b9d6f2, #cbe7c6);
}

.media--lake {
  background: linear-gradient(120deg, #c7d9ff, #f1e7ff);
}

.post-footer {
  display: flex;
  gap: 16px;
  color: var(--ink-500);
  font-size: 0.85rem;
}

.rightbar {
  display: flex;
  flex-direction: column;
  gap: 18px;
}

.right-card {
  padding: 18px;
}

.muted {
  color: var(--ink-500);
  font-size: 0.9rem;
}

.badge {
  background: var(--surface-200);
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.stack {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.note-row,
.community-row {
  display: flex;
  gap: 12px;
  align-items: flex-start;
}

.note-dot {
  width: 10px;
  height: 10px;
  border-radius: 999px;
  background: var(--accent-500);
  margin-top: 6px;
}

.note-title {
  font-weight: 600;
  font-size: 0.9rem;
}

.note-time {
  color: var(--ink-500);
  font-size: 0.75rem;
}

.community-icon {
  width: 36px;
  height: 36px;
  border-radius: 12px;
  background: linear-gradient(135deg, #f5c8a0, #ffd7e4);
}

.community-name {
  font-weight: 600;
}

.community-meta {
  color: var(--ink-500);
  font-size: 0.8rem;
}

@keyframes rise {
  from {
    opacity: 0;
    transform: translateY(12px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@media (max-width: 1200px) {
  .content-grid {
    grid-template-columns: 240px minmax(0, 1fr);
  }

  .rightbar {
    display: none;
  }
}

@media (max-width: 980px) {
  .layout {
    grid-template-columns: 1fr;
  }

  .sidebar {
    position: static;
  }

  .content-grid {
    grid-template-columns: 1fr;
  }

  .topbar {
    flex-direction: column;
    align-items: stretch;
  }

  .topbar-left,
  .topbar-right {
    justify-content: space-between;
  }
}

@media (max-width: 680px) {
  .layout {
    padding: 16px;
  }

  .search {
    min-width: 0;
    flex: 1;
  }

  .friends-panel {
    order: 2;
  }
}
</style>
