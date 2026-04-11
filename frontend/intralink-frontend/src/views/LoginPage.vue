<template>
  <section class="login-page">
    <h1>Login Page</h1>
    <p>Страница входа</p>
    <RouterLink to="/register">Перейти к регистрации</RouterLink>
  </section>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: grid;
  place-content: center;
  gap: 8px;
  padding: 24px;
}
</style>
