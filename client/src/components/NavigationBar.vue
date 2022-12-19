<script setup>
import {ref} from 'vue'
import useAuthenticationStore from '@/stores/authenticationStore'
import LogoutModal from '@/components/Modals/LogoutModal.vue'

const authStore = useAuthenticationStore()

const logoutModalRef = ref()

function onLogoutClick() {
  logoutModalRef.value.show()
}

async function onLogout() {
  await authStore.logout()
}
</script>

<template>
  <LogoutModal @logout="onLogout" ref="logoutModalRef"/>

  <b-container>
    <b-navbar toggleable="lg" type="light" variant="faded">
      <b-navbar-brand href="#" to="/">
        <font-awesome-icon icon="fas fa-book"/>
      </b-navbar-brand>

      <b-navbar-toggle target="navbar-toggle-collapse"/>

      <b-collapse id="navbar-toggle-collapse" is-nav>
        <b-navbar-nav>
          <b-nav-item to="/">Каталог</b-nav-item>
          <b-nav-item v-if="authStore.checkAuth" to="/sales">Список продаж</b-nav-item>
          <b-nav-item v-if="authStore.checkAuth" to="/sellers">Продавцы</b-nav-item>
        </b-navbar-nav>

        <b-navbar-nav class="ms-auto">
          <b-nav-item v-if="!authStore.checkAuth" to="/authorization" right>
            <b-button class="fas fa-user" variant="outline-primary">
              <div class="m-1 ms-2">Войти</div>
            </b-button>
          </b-nav-item>

          <b-nav-item v-else right>
            <b-button class="fas fa-angle-right" variant="outline-primary" @click="onLogoutClick">
              <div class="m-1 ms-2">Выйти</div>
            </b-button>
          </b-nav-item>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </b-container>
</template>

<style scoped>
</style>