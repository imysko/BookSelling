<script setup>
import useSalesStore from '@/stores/salesStore'
import {computed, reactive} from 'vue'
import {FontAwesomeIcon} from '@fortawesome/vue-fontawesome'

const saleStore = useSalesStore()

const filter = reactive({
  sellerName: '',
  sellerSurname: '',
  minDate: {
    value: Date.now(),
    type: Date
  },
  maxDate: {
    value: Date.now(),
    type: Date
  }
})

const ranges = reactive({
  minDate: {
    value: Date.now().toString(),
    type: String
  },
  maxDate: {
    value: Date.now(),
    type: String
  }
})

const computedRanges = computed(() => {
  ranges.minDate = saleStore.sales.minDate
  ranges.maxDate = saleStore.sales.maxDate

  filter.minDate = ranges.minDate
  filter.maxDate = ranges.maxDate

  return ranges
})

async function onApplyFilter() {
  saleStore.filter.sellerName = filter.sellerName
  saleStore.filter.sellerSurname = filter.sellerSurname
  saleStore.filter.minDate = filter.minDate
  saleStore.filter.maxDate = filter.maxDate

  await saleStore.debouncedFetchSales()
}

async function onResetFilter() {
  filter.sellerName = ''
  filter.sellerSurname = ''
  filter.minDate = Date.now()
  filter.maxDate = Date.now()

  await onApplyFilter()
}
</script>

<template>
  <b-card title="Фильтр">
    <b-row class="my-1">
      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.sellerName"
              type="text"
              class="form-control my-2"
              placeholder="sellerName">
          <label for="floatingInput">Имя продавца</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.sellerSurname"
              type="text"
              class="form-control my-2"
              placeholder="sellerSurname">
          <label for="floatingInput">Фамилия продавца</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.minDate"
              type="date"
              :min="computedRanges.minDate"
              :max="filter.maxDate"
              class="form-control my-2"
              placeholder="minDate">
          <label for="floatingInput">Начальная дата</label>
        </div>
      </div>

      <div role="group">
        <div class="form-floating">
          <input
              v-model="filter.maxDate"
              type="date"
              :min="filter.minDate"
              :max="computedRanges.maxDate"
              class="form-control my-2"
              placeholder="author">
          <label for="floatingInput">Конечная дата</label>
        </div>
      </div>

      <div role="group" class="d-flex justify-content-around mt-3">
        <b-button pill variant="outline-danger" @click="onResetFilter">
          <font-awesome-icon icon="fas fa-rotate-left" size="md" />
        </b-button>
        <b-button pill variant="outline-warning" @click="onApplyFilter">Применить</b-button>
      </div>
    </b-row>
  </b-card>
</template>

<style scoped>
</style>