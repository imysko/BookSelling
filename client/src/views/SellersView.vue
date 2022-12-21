<script setup>
import SellersFilter from '@/components/Filters/SellersFilter.vue'
import SellersSorting from '@/components/Sortings/SellersSorting.vue'
import SellersList from '@/components/Lists/SellersList.vue'
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