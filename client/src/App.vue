<script setup>
import NavigationBar from '@/components/NavigationBar.vue'
import { RouterView } from 'vue-router'
</script>

<template>
  <header>
    <NavigationBar/>
  </header>

  <b-container class="pt-2">
    <RouterView />
  </b-container>
</template>

<style scoped>
</style>
