import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginPage from '../views/LoginPage.vue'
import RegisterPage from '../views/RegisterPage.vue'
import ProfileView from '../views/ProfileView.vue'
import { useAuthStore } from '../stores/authStore'
import { getAuthRedirect, getGuestOnlyRedirect } from './authGuard'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: '/login',
      name: 'login',
      component: LoginPage,
      meta: { guestOnly: true },
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterPage,
      meta: { guestOnly: true },
    },
    {
      path: '/profile',
      name: 'profile',
      component: ProfileView,
      meta: { requiresAuth: true },
    }
  ],
})

router.beforeEach((to) => {
  const authStore = useAuthStore()
  const authRedirect = getAuthRedirect(
    Boolean(to.meta.requiresAuth),
    authStore.isAuthenticated,
    to.fullPath,
  )

  if (authRedirect !== true) return authRedirect

  return getGuestOnlyRedirect(Boolean(to.meta.guestOnly), authStore.isAuthenticated)
})

export default router

