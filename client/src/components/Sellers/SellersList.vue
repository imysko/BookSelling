<script setup>
import SellerCard from '@/components/Sellers/SellerCard.vue'
import useSellersStore from '@/stores/sellersStore'

const sellersStore = useSellersStore()
</script>

<template>
  <div v-if="sellersStore.sellers.items.length">
    <SellerCard v-for="seller in sellersStore.sellers.items" :seller="seller"/>

    <b-pagination
        pills
        v-model="sellersStore.filter.currentPage"
        v-on:click="sellersStore.debouncedFetchSellers"
        :total-rows="sellersStore.pageInfo.totalPages * sellersStore.pageInfo.pageSize"
        :per-page="sellersStore.pageInfo.pageSize"
        first-number
        last-number
        align="center">
    </b-pagination>
  </div>
  <div v-else>
    Ничего не найдено!
  </div>
</template>

<style scoped>
</style>