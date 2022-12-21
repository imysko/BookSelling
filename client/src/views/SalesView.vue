<script setup>
import SalesFilter from '@/components/Filters/SalesFilter.vue'
import SalesSorting from '@/components/Sortings/SalesSorting.vue'
import SalesList from '@/components/Lists/SalesList.vue'
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