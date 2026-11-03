type LoginRedirect = {
  name: 'login'
  query: { redirect: string }
}

type HomeRedirect = {
  name: 'home'
}

export function getAuthRedirect(
  requiresAuth: boolean,
  isAuthenticated: boolean,
  fullPath: string,
): LoginRedirect | true {
  if (!requiresAuth || isAuthenticated) return true

  return {
    name: 'login',
    query: { redirect: fullPath },
  }
}

export function getGuestOnlyRedirect(
  guestOnly: boolean,
  isAuthenticated: boolean,
): HomeRedirect | true {
  if (!guestOnly || !isAuthenticated) return true

  return { name: 'home' }
}

export function getPostLoginRedirect(value: unknown): string {
  if (typeof value !== 'string' || !value.startsWith('/') || value.startsWith('//')) {
    return '/'
  }

  return value
}
