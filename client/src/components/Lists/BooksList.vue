<script setup>
import BookCard from '@/components/Cards/BookCard.vue'
import ChangeBookModal from '@/components/Modals/ChangeBookModal.vue'
import useBooksStore from '@/stores/booksStore'
import {ref} from "vue";

const bookStore = useBooksStore()

const changeBookModalRef = ref()

function onEditClick(book) {
  changeBookModalRef.value.handBook(book)
  changeBookModalRef.value.show()
}

async function editBook(book) {
  bookStore.book.id = book.id
  bookStore.book.data = book

  await bookStore.changeBook()
  await bookStore.debouncedFetchBooks()
}

async function onBuyClick(bookId, count) {
  bookStore.buy.bookId = bookId
  bookStore.buy.soldCount = count
  bookStore.buy.date = new Date().toISOString()

  await bookStore.buyBook()
  await bookStore.debouncedFetchBooks()
}
</script>

<template>
  <ChangeBookModal @book-edit="editBook" ref="changeBookModalRef" />

  <div v-if="bookStore.books.items.length">
    <b-card-group>
      <BookCard
          @edit-book-click="onEditClick"
          @buy-book-click="onBuyClick"
          v-for="book in bookStore.books.items"
          :book="book"/>
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