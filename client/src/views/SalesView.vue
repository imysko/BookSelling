<script setup>
import SalesFilter from '@/components/Sales/SalesFilter.vue'
import SalesSorting from '@/components/Sales/SalesSorting.vue'
import SalesList from '@/components/Sales/SalesList.vue'
</script>

<template>
  <b-container>
    <header>
      <h1>Список продаж</h1>
    </header>

    <main>
      <b-row>
        <b-col sm="3">
          <SalesFilter/>
        </b-col>
        <b-col sm="9">
          <SalesSorting class="mb-2"/>
          <SalesList/>
        </b-col>
      </b-row>
    </main>
  </b-container>
</template>

<style scoped>
</style>