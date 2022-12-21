<script setup>
import useBooksStore from '@/stores/booksStore'

const booksStore = useBooksStore()

const sortType = [
  { text: 'По возрастанию', value: 'asc' },
  { text: 'По убыванию', value: 'desc' },
]

const sortColumn = [
  { text: 'По наименованию', value: 'name' },
  { text: 'По автору', value: 'author' },
  { text: 'По году', value: 'year' },
  { text: 'По цене', value: 'price' },
]
</script>

<template>
  <b-container class="d-flex">
    <b-col class="d-flex align-items-center">
      <div class="me-2">Сортировать:</div>
      <b-dropdown variant="warning" :text="sortColumn.find(s => s.value === booksStore.filter.columnToSort).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 190px"
            v-model="booksStore.filter.columnToSort"
            :options="sortColumn"
            stacked
            @change="booksStore.debouncedFetchBooks">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
    <b-col class="d-flex align-items-center">
      <div class="me-2">Тип сортировки:</div>
      <b-dropdown variant="warning" :text="sortType.find(s => s.value === booksStore.filter.sortType).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 170px"
            v-model="booksStore.filter.sortType"
            :options="sortType"
            stacked
            @change="booksStore.debouncedFetchBooks">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
  </b-container>
</template>

<style scoped>
</style>