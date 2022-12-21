<script setup>
import useSalesStore from '@/stores/salesStore'

const salesStore = useSalesStore()

const sortType = [
  { text: 'По возрастанию', value: 'asc' },
  { text: 'По убыванию', value: 'desc' },
]

const sortColumn = [
  { text: 'По имени продавца', value: 'seller_name' },
  { text: 'По дате', value: 'date' },
]
</script>

<template>
  <b-container class="d-flex">
    <b-col class="d-flex align-items-center">
      <div class="me-2">Сортировать:</div>
      <b-dropdown variant="warning" :text="sortColumn.find(s => s.value === salesStore.filter.columnToSort).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 200px"
            v-model="salesStore.filter.columnToSort"
            :options="sortColumn"
            stacked
            @change="salesStore.debouncedFetchSales">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
    <b-col class="d-flex align-items-center">
      <div class="me-2">Тип сортировки:</div>
      <b-dropdown variant="warning" :text="sortType.find(s => s.value === salesStore.filter.sortType).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 170px"
            v-model="salesStore.filter.sortType"
            :options="sortType"
            stacked
            @change="salesStore.debouncedFetchSales">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
  </b-container>
</template>

<style scoped>
</style>