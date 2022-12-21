<script setup>
import SellersFilter from '@/components/Sellers/SellersFilter.vue'
import SellersSorting from '@/components/Sellers/SellersSorting.vue'
import SellersList from '@/components/Sellers/SellersList.vue'
</script>

<template>
  <b-container>
    <header>
      <h1>Список продавцов</h1>
    </header>

    <main>
      <b-row>
        <b-col sm="3">
          <SellersFilter/>
        </b-col>
        <b-col sm="9">
          <SellersSorting class="mb-2"/>
          <SellersList/>
        </b-col>
      </b-row>
    </main>
  </b-container>
</template>

<style scoped>
</style>