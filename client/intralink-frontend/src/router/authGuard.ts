type LoginRedirect = {
  name: 'login'
  query: { redirect: string }
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

export function getPostLoginRedirect(value: unknown): string {
  if (typeof value !== 'string' || !value.startsWith('/') || value.startsWith('//')) {
    return '/'
  }

  return value
}
