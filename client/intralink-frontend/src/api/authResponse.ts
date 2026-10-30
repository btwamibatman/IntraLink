export interface AuthUser {
  id: number
  name: string
  email: string
  role: string
}

export interface AuthResponse {
  user: AuthUser
  token?: string
}

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null && !Array.isArray(value)
}

export function parseAuthResponse(value: unknown): AuthResponse | null {
  if (!isRecord(value)) return null

  let userValue = value
  if (Object.prototype.hasOwnProperty.call(value, 'user')) {
    if (!isRecord(value.user)) return null
    userValue = value.user
  }
  const { id, name, email } = userValue

  if (typeof id !== 'number' || !Number.isInteger(id) || id <= 0) return null
  if (typeof name !== 'string' || typeof email !== 'string') return null

  const normalizedName = name.trim()
  const normalizedEmail = email.trim()
  if (!normalizedName || !/^\S+@\S+\.\S+$/.test(normalizedEmail)) return null

  const role = typeof userValue.role === 'string' && userValue.role.trim()
    ? userValue.role.trim()
    : 'User'
  const token = typeof value.token === 'string' && value.token.trim()
    ? value.token
    : undefined

  return {
    user: {
      id,
      name: normalizedName,
      email: normalizedEmail,
      role,
    },
    ...(token ? { token } : {}),
  }
}
