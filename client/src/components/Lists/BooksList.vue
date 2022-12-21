<script setup>
import BookCard from '@/components/Cards/BookCard.vue'
import useBooksStore from '@/stores/booksStore'

const bookStore = useBooksStore()
</script>

<template>
  <div v-if="bookStore.books.items.length">
    <b-card-group>
      <BookCard v-for="book in bookStore.books.items" :book="book"/>
    </b-card-group>

    <b-pagination
        pills
        v-model="bookStore.filter.currentPage"
        v-on:click="bookStore.debouncedFetchBooks"
        :total-rows="bookStore.pageInfo.totalPages * bookStore.pageInfo.pageSize"
        :per-page="bookStore.pageInfo.pageSize"
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