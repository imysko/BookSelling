<script setup>
import useSellersStore from '@/stores/sellersStore'

const sellersStore = useSellersStore()

const sortType = [
  { text: 'По возрастанию', value: 'asc' },
  { text: 'По убыванию', value: 'desc' },
]

const sortColumn = [
  { text: 'По имени', value: 'name' },
  { text: 'По фамилии', value: 'surname' },
  { text: 'По отчеству', value: 'fname' },
  { text: 'По номеру телефона', value: 'phone_number' },
]
</script>

<template>
  <b-container class="d-flex">
    <b-col class="d-flex align-items-center">
      <div class="me-2">Сортировать:</div>
      <b-dropdown variant="warning" :text="sortColumn.find(s => s.value === sellersStore.filter.columnToSort).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 205px"
            v-model="sellersStore.filter.columnToSort"
            :options="sortColumn"
            stacked
            @change="sellersStore.debouncedFetchSellers">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
    <b-col class="d-flex align-items-center">
      <div class="me-2">Тип сортировки:</div>
      <b-dropdown variant="warning" :text="sortType.find(s => s.value === sellersStore.filter.sortType).text">
        <b-form-radio-group
            class="ps-2"
            style="width: 170px"
            v-model="sellersStore.filter.sortType"
            :options="sortType"
            stacked
            @change="sellersStore.debouncedFetchSellers">
        </b-form-radio-group>
      </b-dropdown>
    </b-col>
  </b-container>
</template>

<style scoped>
</style>