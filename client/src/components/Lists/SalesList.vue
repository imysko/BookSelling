<script setup>
import SaleCard from '@/components/Cards/SaleCard.vue'
import useSalesStore from '@/stores/salesStore'

const salesStore = useSalesStore()
</script>

<template>
  <div v-if="salesStore.sales.items.length">
    <SaleCard v-for="sale in salesStore.sales.items" :sale="sale"/>

    <b-pagination
        pills
        v-model="salesStore.filter.currentPage"
        v-on:click="salesStore.debouncedFetchSales"
        :total-rows="salesStore.pageInfo.totalPages * salesStore.pageInfo.pageSize"
        :per-page="salesStore.pageInfo.pageSize"
        first-number
        last-number
        align="center">
    </b-pagination>
  </div>
  <div v-else class="d-flex justify-content-center">
    Ничего не найдено!
  </div>
</template>

<style scoped>
</style>