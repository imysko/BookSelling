<script setup>
import BooksFilter from '@/components/Books/BooksFilter.vue'
import BooksSorting from '@/components/Books/BooksSorting.vue'
import BooksList from '@/components/Books/BooksList.vue'
</script>

<template>
  <b-container>
    <header>
      <h1>Каталог книг</h1>
    </header>

    <main>
      <b-row>
        <b-col sm="3">
          <BooksFilter/>
        </b-col>
        <b-col sm="9">
          <BooksSorting class="mb-2"/>
          <BooksList/>
        </b-col>
      </b-row>
    </main>
  </b-container>
</template>
